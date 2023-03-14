using Battery.Framework;
using Battery.Engine;
using OpenGL;
using ImGuiNET;
using System.Numerics;

namespace Battery.Tools;

public class GuiManager : GameManager
{
    private ImGuiIOPtr _io => ImGui.GetIO();
    private Game _game;
    private IntPtr _fontsTexture;

    private Mesh<ImGuiVertex>? _mesh;

    private Shader? _shader;

    private ShaderMaterial? _material;

    internal static VertexDescription _description = new VertexDescription(
        new VertexAttribute("Position", VertexAttributeType.Float, 2, false),
        new VertexAttribute("UV",       VertexAttributeType.Float, 2, false),
        new VertexAttribute("Color",    VertexAttributeType.Byte,  4, true)
    );

    public struct ImGuiVertex : IVertex
    {
        public Vector2 pos;
        public Vector2 uv;
        public uint col;
        public VertexDescription Description => _description;
    }

    /// <summary>
    ///     The Vertex Shader used by the OpenGL graphics.
    /// </summary>
    public static string GLVertexShader = @"
        #version 330
        
        precision mediump float;
        layout (location = 0) in vec2 Position;
        layout (location = 1) in vec2 UV;
        layout (location = 2) in vec4 Color;
        uniform mat4 ProjMtx;
        out vec2 Frag_UV;
        out vec4 Frag_Color;

        void main()
        {
            Frag_UV = UV;
            Frag_Color = Color;
            gl_Position = ProjMtx * vec4(Position.xy, 0, 1);
        }";

    /// <summary>
    ///     The Fragment Shader used by the OpenGL graphics.
    /// </summary>
    public static string GLFragmentShader = @"
        #version 330
                
        precision mediump float;
        uniform sampler2D Texture;
        in vec2 Frag_UV;
        in vec4 Frag_Color;
        layout (location = 0) out vec4 Out_Color;
        
        void main()
        {
            Out_Color = Frag_Color * texture(Texture, Frag_UV.st);
        }";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="instance"></param>
    public GuiManager(Game instance)
    {
        _game = instance;
    }

    /// <summary>
    ///     Begins the ImGui, by creating the context, loading the resources and initializing the input.
    /// </summary>
    public override void Begin()
    {
        ImGui.SetCurrentContext(ImGui.CreateContext());
        RebuildFontAtlas();
        BeginKeyMap();

        Keyboard.OnKeyDown += (KeyConstant key) => _io.KeysDown[(int)key] = true;
        Keyboard.OnKeyUp   += (KeyConstant key) => _io.KeysDown[(int)key] = false;
    
        Mouse.OnMouseMoved += (Vector2 position) => _io.MousePos = position;
        Mouse.OnWheelMoved += (Vector2 position) =>
        {
            if (position.X > 0) _io.MouseWheelH += 1;
            if (position.X < 0) _io.MouseWheelH -= 1;   
            if (position.Y > 0) _io.MouseWheel += 1;
            if (position.Y < 0) _io.MouseWheel -= 1;
        };
    }

    /// <summary>
    ///     Presents the Draw Data to the Screen.
    /// </summary>
    public override void Render()
    {
        NewFrame();
        RenderContent();
        ImGui.Render();
        Present();
    }

    /// <summary>
    ///     Renders the ImGui Content.
    /// </summary>
    public virtual void RenderContent()
    {
        ImGui.ShowDemoWindow();
    }

    /// <summary>
    ///     Updates the current ImGui state.
    /// </summary>
    public void NewFrame()
    {
        ImGui.NewFrame();
        _io.DisplaySize = _game.Platform.Dimensions;
        _io.DeltaTime   = Time.RawDelta;

        UpdateMouseState();
    }

    /// <summary>
    ///     Presents the Graphics Data.
    /// </summary>
    public void Present()
    {
        // Clears the window background.
        _game.Graphics.Clear(_game.Platform, Color.Black);

        // Creates the Shader and its Material.
        if ((_shader == null || _material == null) && _game.Graphics is OpenGLGraphics)
        {
            _shader = _game.Graphics.CreateShader((GLVertexShader, GLFragmentShader));
            _material = new ShaderMaterial(_shader);
        }
        else
            throw new NotImplementedException(_game.Graphics.GetType().Name);

        // Starts the rendering state.
        var drawData = ImGui.GetDrawData();

        var fbWidth  = (int)(drawData.DisplaySize.X * drawData.FramebufferScale.X);
        var fbHeight = (int)(drawData.DisplaySize.Y * drawData.FramebufferScale.Y);
        if (fbWidth <= 0 || fbHeight <= 0)
            return;

        var clipOffset = drawData.DisplayPos;
        var clipScale  = drawData.FramebufferScale;
        drawData.ScaleClipRects(clipScale);

        // Assign the Shader Uniforms.
        var left   = drawData.DisplayPos.X;
        var right  = drawData.DisplayPos.X + drawData.DisplaySize.X;
        var top    = drawData.DisplayPos.Y;
        var bottom = drawData.DisplayPos.Y + drawData.DisplaySize.Y;

        _material.SetUniform("Texture", 0);
        _material.SetUniform("ProjMtx", Matrix4x4.CreateOrthographicOffCenter(left, right, bottom, top, -1, 1));

        // Bind the font texture.
        var lastTexId = ImGui.GetIO().Fonts.TexID;
        GL.glBindTexture(GL.GL_TEXTURE_2D, (uint)lastTexId);

        // If there is no mesh, create a new one.
        if (_mesh == null)
            _mesh = _game.Graphics.CreateMesh<ImGuiVertex>();

        // Creates the render pass.
        var _pass = new RenderPass<ImGuiVertex>(_game.Platform, _mesh, _material);
        _pass.Viewport = new RectangleI(
            0,
            0, 
            (int)_io.DisplaySize.X,
            (int)_io.DisplaySize.Y
        );
        
        // Begin the renderization.
        for (var n = 0; n < drawData.CmdListsCount; n++)
        {
            var cmdList = drawData.CmdListsRange[n];
            for (var i = 0; i < cmdList.VtxBuffer.Capacity; i++)
            {
                // Upload the vertices to the mesh.
                _mesh.AddVertex(
                    new ImGuiVertex 
                    { 
                        pos = cmdList.VtxBuffer[i].pos,
                        uv  = cmdList.VtxBuffer[i].uv,
                        col = cmdList.VtxBuffer[i].col,
                    }
                );
            }

            for (var i = 0; i < cmdList.IdxBuffer.Capacity; i++)
            {
                // Upload the indices to the mesh.
                _mesh.AddIndex((uint)cmdList.IdxBuffer[i]);
            }

            for (var cmd_i = 0; cmd_i < cmdList.CmdBuffer.Size; cmd_i++)
            {
                var pcmd = cmdList.CmdBuffer[cmd_i];
                if (pcmd.UserCallback != IntPtr.Zero)
                {
                    Console.WriteLine("UserCallback not implemented");
                }
                else
                {
                    // Project scissor/clipping rectangles into framebuffer space.
                    var clip_rect = pcmd.ClipRect;

                    clip_rect.X = pcmd.ClipRect.X - clipOffset.X;
                    clip_rect.Y = pcmd.ClipRect.Y - clipOffset.Y;
                    clip_rect.Z = pcmd.ClipRect.Z - clipOffset.X;
                    clip_rect.W = pcmd.ClipRect.W - clipOffset.Y;

                    _pass.Scissor = new(
                        (int)clip_rect.X, 
                        (int)(fbHeight - clip_rect.W), 
                        (int)(clip_rect.Z - clip_rect.X), 
                        (int)(clip_rect.W - clip_rect.Y)
                    );

                    // Bind texture, draw.
                    if (pcmd.TextureId != IntPtr.Zero)
                    {
                        if (pcmd.TextureId != lastTexId)
                        {
                            lastTexId = pcmd.TextureId;
                            GL.glBindTexture(GL.GL_TEXTURE_2D, (uint)pcmd.TextureId);
                        }
                    }

                    // Presents the Draw Data.
                    _pass.IndexStart = (int)pcmd.IdxOffset;
                    _pass.IndexCount = (int)pcmd.ElemCount;
                    _pass.pruigi = (int)pcmd.VtxOffset;
                    
                    _game.Graphics.Present(_pass);
                }
            }
        }

        _mesh.Clear();
    }

    #region Input

    /// <summary>
    ///     Assign the ImGui Keys.
    /// </summary>
    public void BeginKeyMap()
    {
        _io.KeyMap[(int)ImGuiKey.Tab]            = (int)KeyConstant.Tab;
        _io.KeyMap[(int)ImGuiKey.LeftArrow]      = (int)KeyConstant.Left;
        _io.KeyMap[(int)ImGuiKey.RightArrow]     = (int)KeyConstant.Right;
        _io.KeyMap[(int)ImGuiKey.UpArrow]        = (int)KeyConstant.Up;
        _io.KeyMap[(int)ImGuiKey.DownArrow]      = (int)KeyConstant.Down;
        _io.KeyMap[(int)ImGuiKey.PageUp]         = (int)KeyConstant.PageUp;
        _io.KeyMap[(int)ImGuiKey.PageDown]       = (int)KeyConstant.PageDown;
        _io.KeyMap[(int)ImGuiKey.Home]           = (int)KeyConstant.Home;
        _io.KeyMap[(int)ImGuiKey.End]            = (int)KeyConstant.End;
        _io.KeyMap[(int)ImGuiKey.Insert]         = (int)KeyConstant.Insert;
        _io.KeyMap[(int)ImGuiKey.Delete]         = (int)KeyConstant.Delete;
        _io.KeyMap[(int)ImGuiKey.Backspace]      = (int)KeyConstant.Backspace;
        _io.KeyMap[(int)ImGuiKey.Space]          = (int)KeyConstant.Space;
        _io.KeyMap[(int)ImGuiKey.Enter]          = (int)KeyConstant.Return;
        _io.KeyMap[(int)ImGuiKey.Escape]         = (int)KeyConstant.Escape;

        _io.KeyMap[(int)ImGuiKey.A] = (int)KeyConstant.A;
        _io.KeyMap[(int)ImGuiKey.B] = (int)KeyConstant.B;
        _io.KeyMap[(int)ImGuiKey.C] = (int)KeyConstant.C;
        _io.KeyMap[(int)ImGuiKey.D] = (int)KeyConstant.D;
        _io.KeyMap[(int)ImGuiKey.E] = (int)KeyConstant.E;
        _io.KeyMap[(int)ImGuiKey.F] = (int)KeyConstant.F;
        _io.KeyMap[(int)ImGuiKey.G] = (int)KeyConstant.G;
        _io.KeyMap[(int)ImGuiKey.H] = (int)KeyConstant.H;
        _io.KeyMap[(int)ImGuiKey.I] = (int)KeyConstant.I;
        _io.KeyMap[(int)ImGuiKey.J] = (int)KeyConstant.J;
        _io.KeyMap[(int)ImGuiKey.K] = (int)KeyConstant.K;
        _io.KeyMap[(int)ImGuiKey.L] = (int)KeyConstant.L;
        _io.KeyMap[(int)ImGuiKey.M] = (int)KeyConstant.M;
        _io.KeyMap[(int)ImGuiKey.N] = (int)KeyConstant.N;
        _io.KeyMap[(int)ImGuiKey.O] = (int)KeyConstant.O;
        _io.KeyMap[(int)ImGuiKey.P] = (int)KeyConstant.P;
        _io.KeyMap[(int)ImGuiKey.Q] = (int)KeyConstant.Q;
        _io.KeyMap[(int)ImGuiKey.R] = (int)KeyConstant.R;
        _io.KeyMap[(int)ImGuiKey.S] = (int)KeyConstant.S;
        _io.KeyMap[(int)ImGuiKey.T] = (int)KeyConstant.T;
        _io.KeyMap[(int)ImGuiKey.U] = (int)KeyConstant.U;
        _io.KeyMap[(int)ImGuiKey.V] = (int)KeyConstant.V;
        _io.KeyMap[(int)ImGuiKey.W] = (int)KeyConstant.W;
        _io.KeyMap[(int)ImGuiKey.X] = (int)KeyConstant.X;
        _io.KeyMap[(int)ImGuiKey.Y] = (int)KeyConstant.Y;
        _io.KeyMap[(int)ImGuiKey.Z] = (int)KeyConstant.Z;

        _io.KeyMap[(int)ImGuiKey.LeftShift]  = (int)KeyConstant.LeftShift;
        _io.KeyMap[(int)ImGuiKey.RightShift] = (int)KeyConstant.RightShift;
    }

    /// <summary>
    ///     Updates the actual mouse state.
    /// </summary>
    public void UpdateMouseState()
    {
        _io.MouseDown[0] = Mouse.Down(MouseButton.Left);
        _io.MouseDown[1] = Mouse.Down(MouseButton.Right);
        _io.MouseDown[2] = Mouse.Down(MouseButton.Middle);
    }

    #endregion

    #region Assets

    /// <summary>
    ///     Reload all the Fonts texture atlas.
    /// </summary>
    public unsafe void RebuildFontAtlas()
    {
        var fonts = ImGui.GetIO().Fonts;
        fonts.AddFontDefault();
        fonts.GetTexDataAsRGBA32(out byte* pixels, out int width, out int height);

        fonts.TexID = _fontsTexture = (IntPtr)LoadTexture((IntPtr)pixels, width, height);

        fonts.ClearTexData();
    }

    /// <summary>
    ///     Loads a texture with the given dimensions by using the specified pixels pointer.
    /// </summary>
    /// <param name="pixels"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    private uint LoadTexture(IntPtr pixels, int width, int height)
    {
        if (_game.Graphics is OpenGLGraphics glGraphics)
        {
            var id = GL.glGenTexture();
            GL.glPixelStorei(GL.GL_UNPACK_ALIGNMENT, 1);
            GL.glBindTexture(GL.GL_TEXTURE_2D, id);

            GL.glTexImage2D(
                GL.GL_TEXTURE_2D,
                0,
                GL.GL_RGBA,
                width,
                height,
                0,
                GL.GL_RGBA,
                GL.GL_UNSIGNED_BYTE,
                pixels
            );

            GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, GL.GL_LINEAR);
            GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, GL.GL_LINEAR);
            GL.glBindTexture(GL.GL_TEXTURE_2D, 0);
            
            return id;
        }

        throw new NotImplementedException("Graphics Backend not implemented yet.");
    }

    #endregion
}