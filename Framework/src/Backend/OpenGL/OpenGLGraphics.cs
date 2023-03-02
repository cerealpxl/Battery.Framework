using SDL2;
using OpenGL;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Battery.Framework;

/// <summary>
///     A OpenGl Graphics Backend.
/// </summary>
public class OpenGLGraphics : GameGraphics
{
    /// The pointer to the OpenGl Context.
    internal IntPtr _glPointer;

    /// <summary>
    ///     Creates a new instance of the <see cref="OpenGLGraphics" /> class.
    /// </summary>
    /// <param name="instance">The game to which the graphics belongs to.</param>
    public OpenGLGraphics(Game instance)
        : base(instance)
    {
    }

    /// <inheritdoc />
    public override Shader CreateShader(string vertex, string fragment)
        => new OpenGLShader(vertex, fragment);

    /// <inheritdoc />
    public override Shader CreateDefaultShader()
    {
        return new OpenGLShader(
        @"
            #version 330 core
            layout (location = 0) in vec2 in_Position;
            layout (location = 1) in vec2 in_TexCoord;
            layout (location = 2) in vec4 in_Color;
            layout (location = 3) in vec3 in_Type;

            uniform mat4 u_Matrix;

            out vec2 out_TexCoord;
            out vec4 out_Color;
            out vec3 out_Type;

            void main()
            {
                gl_Position = u_Matrix * vec4(in_Position.x, in_Position.y, 0.0, 1.0);
                out_TexCoord = in_TexCoord;
                out_Color    = in_Color;
                out_Type     = in_Type;
            }
        ",
        @"
            #version 330 core

            in vec2 out_TexCoord;
            in vec4 out_Color;
            in vec3 out_Type;

            out vec4 r_Color;

            uniform sampler2D u_Texture;

            void main()
            {
                vec4 color = texture(u_Texture, out_TexCoord);
                r_Color = 
                    out_Type.x * color   * out_Color + 
                    out_Type.y * color.a * out_Color + 
                    out_Type.z           * out_Color;
            }
        "
        );
    }

    /// <inheritdoc />
    public override Mesh CreateMesh()
        => new OpenGLMesh();

    /// <inheritdoc />
    public override Texture CreateTexture(Bitmap bitmap)
        => new OpenGLTexture(bitmap);

    /// <summary>
    ///     Begins the OpenGL Graphics.
    /// </summary>
    public override void Begin()
    {
        if (Game.Platform is SDLPlatform sdlPlatform)
        {
            // Initializes the OpenGl Context in the SDL window.
            _glPointer = SDL.SDL_GL_CreateContext(sdlPlatform._window);

            // Error handling.
            if (_glPointer == IntPtr.Zero)
                throw new Exception(SDL.SDL_GetError());

            // Finally, initialize the OpenGl.
            SDL.SDL_GL_MakeCurrent(sdlPlatform._window, _glPointer);
            GL.Import(SDL.SDL_GL_GetProcAddress);
        }
        else
            throw new Exception("The Window Platform does not supports the OpenGl.");

        // Update the OpenGL draw settings.
        GL.glDisable(GL.GL_DEPTH_TEST);
        GL.glEnable(GL.GL_BLEND);
        GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
    }

    /// <summary>
    ///     Ends the OpenGl graphics.
    /// </summary>
    public override void End()
        => SDL.SDL_GL_DeleteContext(_glPointer);

    /// <inheritdoc />
    public override void Present(RenderPass pass)
    {
        GL.glBindFramebuffer(0u);

        // Clear the current framebuffer.
        if (pass.ClearColor.A > 0)
        {
            GL.glClearColor(
                pass.ClearColor.R / 255,
                pass.ClearColor.G / 255,
                pass.ClearColor.B / 255,
                pass.ClearColor.A / 255
            );
            GL.glClear(GL.GL_COLOR_BUFFER_BIT);
        }

        // Bind the shader and its uniforms.
        if (pass.Material.Shader is OpenGLShader shader)
        {
            shader.Bind();

            // Sets the shader uniforms.
            foreach (var uniform in pass.Material._uniforms)
            {
                // Gets the uniform location.
                var location = GL.glGetUniformLocation(shader._id, uniform.Key);

                if (uniform.Value is Matrix4x4 mat4x4) 
                {
                    var matrix = new float[16] {
                        mat4x4.M11, mat4x4.M12, mat4x4.M13, mat4x4.M14,
                        mat4x4.M21, mat4x4.M22, mat4x4.M23, mat4x4.M24,
                        mat4x4.M31, mat4x4.M32, mat4x4.M33, mat4x4.M34,
                        mat4x4.M41, mat4x4.M42, mat4x4.M43, mat4x4.M44
                    };

                    unsafe
                    {
                        fixed (float* pointer = &matrix[0])
                            GL.glUniformMatrix4fv(location, 1, false, pointer);
                    }
                }
                else if (uniform.Value is Matrix3x2 mat3x2)
                {
                    var matrix = new float[6] {
                        mat3x2.M11, mat3x2.M12,
                        mat3x2.M21, mat3x2.M22,
                        mat3x2.M31, mat3x2.M32
                    };

                    unsafe
                    {
                        fixed (float* pointer = &matrix[0])
                            GL.glUniformMatrix3x2fv(location, 1, false, pointer);
                    }
                }
                else if (uniform.Value is Vector2 vec2)
                {
                    GL.glUniform2f(location, vec2.X, vec2.Y);
                }
                else if (uniform.Value is Vector3 vec3)
                {
                    GL.glUniform3f(location, vec3.X, vec3.Y, vec3.Z);
                }
                else if (uniform.Value is uint integer)
                {
                    GL.glUniform1ui(location, integer);
                }
                else if (uniform.Value is Color color)
                {
                    GL.glUniform4f(location, color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
                }
                else if (uniform.Value is OpenGLTexture texture)
                {
                    GL.glActiveTexture(GL.GL_TEXTURE0);
                    GL.glBindTexture(GL.GL_TEXTURE_2D, texture.ID);
                    GL.glUniform1i(location, 0);
                }
            }
        }

        // Draw the elements of the mesh.
        if (pass.Mesh is OpenGLMesh mesh) unsafe 
        {
            mesh.Bind();

            // Bind the vertex attribute.
            var size = Marshal.SizeOf<Vertex>();

            GL.glVertexAttribPointer(0, 2, GL.GL_FLOAT,         false, size, GL.NULL);
            GL.glVertexAttribPointer(1, 2, GL.GL_FLOAT,         false, size, new IntPtr(2 * sizeof(float)).ToPointer());
            GL.glVertexAttribPointer(2, 4, GL.GL_UNSIGNED_BYTE, true,  size, new IntPtr(4 * sizeof(float)).ToPointer());
            GL.glVertexAttribPointer(3, 3, GL.GL_UNSIGNED_BYTE, true,  size, new IntPtr(4 * sizeof(float) + 4 * sizeof(byte)).ToPointer());
            GL.glEnableVertexAttribArray(0);
            GL.glEnableVertexAttribArray(1);
            GL.glEnableVertexAttribArray(2);
            GL.glEnableVertexAttribArray(3);

            // Finally. draw the elements.
            GL.glDrawElements(GL.GL_TRIANGLES, pass.IndexCount, GL.GL_UNSIGNED_INT, new IntPtr(sizeof(uint) * pass.IndexStart).ToPointer());   

            // Unbind the mesh.
            GL.glBindBuffer(GL.GL_ELEMENT_ARRAY_BUFFER, 0u);
            GL.glBindBuffer(GL.GL_ARRAY_BUFFER, 0u);
            GL.glBindVertexArray(0u);
        }
    }
}