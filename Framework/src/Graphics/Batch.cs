using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Battery.Framework;

/// <summary>
///     A structure that stores data shared by some vertices.
/// </summary>
internal struct BatchItem
{
    /// <summary>
    ///     Matrix used to transform the vertices.
    /// </summary>
    public Matrix3x2 Matrix;

    /// <summary>
    ///     Material to use.
    /// </summary>
    public ShaderMaterial? Material = null;

    /// <summary>
    ///     Texture to use.
    /// </summary>
    public Texture? Texture;

    /// <summary>
    ///     Render Target to use.
    /// </summary>
    public RenderTarget Target;
    
    /// <summary>
    ///     First vertex to be rendered.
    /// </summary>
    public int IndexStart;
    
    /// <summary>
    ///     Number of vertices in the batch.
    /// </summary>
    public int IndexCount;
    
    /// <summary>
    ///     Creates a new batch.
    /// </summary>
    /// <param name="matrix">The matrix used to render.</param>
    /// <param name="material">The material used to render.</param>
    /// <param name="texture">The texture used to render.</param>
    /// <param name="target">The target used to render.</param>
    /// <param name="start">The first vertex to render.</param>
    /// <param name="count">The number of vertices to render.</param>
    public BatchItem(Matrix3x2? matrix, ShaderMaterial? material, Texture? texture, RenderTarget target, int start, int count)
    {
        Matrix     = matrix ?? Matrix3x2.Identity;
        Material   = material;
        Texture    = texture;
        Target     = target;
        IndexStart = start;
        IndexCount = count;
    }
}

/// <summary>
///     A 2D batch that draw shapes and images.
/// </summary>
public class Batch 
{
    /// <summary>
    ///     Default vertex used for rendering to the screen.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vertex : IVertex
    {
        /// <inheritdoc />
        public VertexDescription Description => Batch.Description;

        /// <summary>
        ///     The position of the vertex.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        ///     The texture coordinate of the vertex.
        /// </summary>
        public Vector2 TexCoord;

        /// <summary>
        ///     The color of the vertex.
        /// </summary>
        public Color Color;
        
        /// <summary>
        ///     Value used to multiply the color of the texture.
        ///     Assigned as 255 when drawing textures.
        /// </summary>
        public byte Mult;

        /// <summary>
        ///     Value used to multiply the color of the texture.
        ///     Assigned as 255 when drawing washed textures.
        /// </summary>
        public byte Wash;

        /// <summary>
        ///     Value used to multiply the color of the vertex. 
        ///     Assigned as 255 when drawing shapes.
        /// </summary>
        public byte Fill;
        
        /// <summary>
        ///     Create a new vertex.
        /// </summary>
        /// <param name="position">The position of the vertex.</param>
        /// <param name="texCoord">The texture coordinate of the vertex.</param>
        /// <param name="color">The color of the vertex.</param>
        public Vertex(Vector2 position, Vector2 texCoord, Color color, int mult, int wash, int fill)
        {
            Position = position;
            TexCoord = texCoord;
            Color    = color;
            Mult     = (byte)mult;
            Wash     = (byte)wash;
            Fill     = (byte)fill;
        }
    }

    /// <summary>
    ///     The description of the default batch vertex.
    /// </summary>
    public static VertexDescription Description = new VertexDescription(
        new VertexAttribute("in_Position", VertexAttributeType.Float, 2),
        new VertexAttribute("in_TexCoord", VertexAttributeType.Float, 2),
        new VertexAttribute("in_Color",    VertexAttributeType.Byte,  4, true),
        new VertexAttribute("in_Type",     VertexAttributeType.Byte,  3, true)
    );

    /// <summary>
    ///     Default shader material used by the batcher.
    /// </summary>
    public ShaderMaterial DefaultShaderMaterial { get; private set; }

    /// <summary>
    ///     Matrix used to transform the drawing shapes.
    /// </summary>
    public Matrix3x2 Matrix = Matrix3x2.Identity;

    /// <summary>
    ///     The number of vertices in the batcher.
    /// </summary>
    public int VertexCount => _mesh.VertexCount;
    
    /// <summary>
    ///     The number of indices in the batcher.
    /// </summary>
    public int IndexCount => _mesh.IndexCount;

    /// <summary>
    ///     The number of batches in the batcher.
    /// </summary>
    public int BatchCount => _items.Count();

    /// <summary>
    ///     The graphics backend to which the batch belongs to.
    /// </summary>
    public Game Game;

    // The mesh used by the batch.
    internal Mesh<Vertex> _mesh;

    // The list that store all the batch items.
    internal readonly List<BatchItem> _items = new List<BatchItem>();

    // A stack that store the pushed matrices.
    private Stack<Matrix3x2> _matrixStack = new Stack<Matrix3x2>();

    // A stack that store the pushed render targets.
    private Stack<RenderTarget> _targetStack = new Stack<RenderTarget>();

    // The index of the current batch in the batch list.
    private int _batchInsert;

    // The current batch used to store the vertex data.
    private BatchItem _batch;

    // The render pass to use.
    private RenderPass<Vertex> _pass;

    // The vertices used when rendering.
    private Vertex[] _pixels = {
        new Vertex(new Vector2(0, 0), new Vector2(0, 0), new Color(0, 0, 0, 0), 0, 0, 0),
        new Vertex(new Vector2(0, 0), new Vector2(0, 0), new Color(0, 0, 0, 0), 0, 0, 0),
        new Vertex(new Vector2(0, 0), new Vector2(0, 0), new Color(0, 0, 0, 0), 0, 0, 0),
        new Vertex(new Vector2(0, 0), new Vector2(0, 0), new Color(0, 0, 0, 0), 0, 0, 0)
    };

    /// <summary>
    ///     Creates a new instance of the <see cref="Batch" /> class.
    /// </summary>
    /// <param name="graphics">The current graphics backend.</param>
    public Batch(Game game)
    {
        Game = game;
        DefaultShaderMaterial = new ShaderMaterial(Game.Graphics.CreateDefaultShader());

        _mesh = Game.Graphics.CreateMesh<Vertex>();
        _pass = new RenderPass<Vertex>(Game.Platform, _mesh, DefaultShaderMaterial);
    }

    /// <summary>
    ///     Clears the batch.
    /// </summary>
    public void Clear()
    {
        _batch       = new BatchItem(null, null, null, Game.Platform, 0, 0);
        _batchInsert = 0;
        
        _matrixStack.Clear();
        _targetStack.Clear();
        _mesh.Clear();
        _items.Clear();

        Matrix = Matrix3x2.Identity;
    }
    
    /// <summary>
    ///     Presents the batch to display the rendered contents.
    /// </summary>
    public void Present()
    {
        if (_items.Count > 0 || _batch.IndexCount > 0)
        {
            /// <summary>
            ///     Function used to draw a single batch item to the screen.
            /// </summary>
            /// <param name="batch">BatchItem to draw.</param>
            /// <param name="matrix">Matrix to use.</param>
            void PerformDraw(BatchItem batch)
            {
                var mat4x4 = Matrix4x4.CreateOrthographicOffCenter(
                    0, 
                    batch.Target.Width,
                    batch.Target.Height,
                    0,
                    0,
                    1
                );
                
                _pass.Material = batch.Material ?? DefaultShaderMaterial;
                _pass.Material.SetUniform("u_Matrix", new Matrix4x4(batch.Matrix) * mat4x4);
                _pass.Material.SetUniform("u_Texture", batch.Texture);

                _pass.Target     = batch.Target;
                _pass.IndexStart = batch.IndexStart;
                _pass.IndexCount = batch.IndexCount;

                Game.Graphics.Present(_pass);
            }

            // Loop for each batch in the list.
            for (int i = 0; i < _items.Count; i ++)
            {
                // Draws the remaining indices in the current batch.
                if (_batchInsert == i && _batch.IndexCount > 0)
                    PerformDraw(_batch);

                // Draws the batch.
                PerformDraw(_items[i]);
            }

            // Draws the remaining indices in the current batch.
            if (_batchInsert == _items.Count && _batch.IndexCount > 0)
                PerformDraw(_batch);
        }
    }
    
    #region Changing the render state

    /// <summary>
    ///     Sets the current texture.
    /// </summary>
    /// <param name="texture">Texture to use.</param>
    public void SetTexture(Texture? texture)
    {
        if (_batch.Texture == null || _batch.IndexCount == 0)
        {
            _batch.Texture = texture;
        }
        else if (_batch.Texture != texture)
        {
            _items.Insert(_batchInsert, _batch);

            _batch.Texture     = texture;
            _batch.IndexStart += _batch.IndexCount;
            _batch.IndexCount  = 0;

            _batchInsert ++;
        }
    }

    /// <summary>
    ///     Sets the current material.
    /// </summary>
    /// <param name="material">Material to use.</param>
    public void SetMaterial(ShaderMaterial? material)
    {
        if (_batch.Material == null || _batch.IndexCount == 0)
        {
            _batch.Material = material;
        }
        else if (_batch.Material != material)
        {
            _items.Insert(_batchInsert, _batch);

            _batch.Material    = material;
            _batch.IndexStart += _batch.IndexCount;
            _batch.IndexCount  = 0;

            _batchInsert ++;
        }
    }

    /// <summary>
    ///     Sets the current render target.
    /// </summary>
    /// <param name="target">RenderTarget to use.</param>
    public void SetRenderTarget(RenderTarget target)
    {
        if (_batch.IndexCount == 0)
        {
            _batch.Target = target;
        }
        else if (_batch.Target != target)
        {
            _items.Insert(_batchInsert, _batch);

            _batch.Target      = target;
            _batch.IndexStart += _batch.IndexCount;
            _batch.IndexCount  = 0;

            _batchInsert ++;
        }
    }

    #endregion
    
    #region Draws a Quad

    /// <summary>
    ///     Draws a quad with the Shape rendering parameters.
    /// </summary>
    /// <param name="v0">The position of the first quad vertex.</param>
    /// <param name="v1">The position of the second quad vertex.</param>
    /// <param name="v2">The position of the third quad vertex.</param>
    /// <param name="v3">The position of the fourth quad vertex.</param>
    /// <param name="color">The color used to renders the quad.</param>
    public void Quad(in Vector2 v0, in Vector2 v1, in Vector2 v2, in Vector2 v3, Color color)
    {
        _batch.IndexCount += 6;

        // Position of the quad.
        _pixels[0].Position = Matrix.MultiplyVector(v0);
        _pixels[1].Position = Matrix.MultiplyVector(v1);
        _pixels[2].Position = Matrix.MultiplyVector(v2);
        _pixels[3].Position = Matrix.MultiplyVector(v3);

        // Color of the quad.
        _pixels[0].Color = 
        _pixels[1].Color = 
        _pixels[2].Color = 
        _pixels[3].Color = color;

        // The vertex multipliers.
        _pixels[0].Mult = 
        _pixels[1].Mult = 
        _pixels[2].Mult = 
        _pixels[3].Mult = 0;

        _pixels[0].Wash = 
        _pixels[1].Wash = 
        _pixels[2].Wash = 
        _pixels[3].Wash = 0;

        _pixels[0].Fill = 
        _pixels[1].Fill = 
        _pixels[2].Fill = 
        _pixels[3].Fill = 255;

        _mesh.AddQuad(ref _pixels[0], ref _pixels[1], ref _pixels[2], ref _pixels[3]);
    }

    /// <summary>
    ///     Draws a quad with the Texture rendering parameters.
    /// </summary>
    /// <param name="v0">The position of the first quad vertex.</param>
    /// <param name="v1">The position of the second quad vertex.</param>
    /// <param name="v2">The position of the third quad vertex.</param>
    /// <param name="v3">The position of the fourth quad vertex.</param>
    /// <param name="color">The color used to renders the quad.</param>
    public void Quad(in Vector2 v0, in Vector2 v1, in Vector2 v2, in Vector2 v3, in Vector2 a, in Vector2 b, in Vector2 c, in Vector2 d, Color color, bool washed = false)
    {
        _batch.IndexCount += 6;
        
        // Position of the quad.
        _pixels[0].Position = Matrix.MultiplyVector(v0);
        _pixels[1].Position = Matrix.MultiplyVector(v1);
        _pixels[2].Position = Matrix.MultiplyVector(v2);
        _pixels[3].Position = Matrix.MultiplyVector(v3);

        // Color of the quad.
        _pixels[0].Color = 
        _pixels[1].Color = 
        _pixels[2].Color = 
        _pixels[3].Color = color;

        // Texture coordinates.
        _pixels[0].TexCoord = a;
        _pixels[1].TexCoord = b;
        _pixels[2].TexCoord = c;
        _pixels[3].TexCoord = d;

        _pixels[0].Mult =
        _pixels[1].Mult =
        _pixels[2].Mult =
        _pixels[3].Mult = (byte)(washed ? 0 : 255);

        _pixels[0].Wash =
        _pixels[1].Wash =
        _pixels[2].Wash =
        _pixels[3].Wash = (byte)(washed ? 255 : 0);

        _pixels[0].Fill =
        _pixels[1].Fill =
        _pixels[2].Fill =
        _pixels[3].Fill = 0;

        _mesh.AddQuad(ref _pixels[0], ref _pixels[1], ref _pixels[2], ref _pixels[3]);
    }

    #endregion
 
    #region Draws a Line
    
    /// <summary>
    ///     Draws a line.
    /// </summary>
    /// <param name="from">Start point of the line.</param>
    /// <param name="to">Target point of the line.</param>
    /// <param name="color">Color of the line.</param>
    /// <param name="thickness">Thickness of the line.</param>
    public void Line(Vector2 from, Vector2 to, Color color, float thickness = 1f)
    {
        var normal        = Vector2.Normalize(from - to);
        var perpendicular = normal.TurnLeft() * thickness * .5f;

        Quad(
            from + perpendicular,
            to + perpendicular,
            to - perpendicular,
            from - perpendicular,
            color
        );
    }

    /// <summary>
    ///     Draws lines.
    /// </summary>
    /// <param name="points">Array storing the line points.</param>
    /// <param name="color">Color of the lines.</param>
    /// <param name="thickness">Thickness of the line.</param>
    public void Line(Vector2[] points, Color color, float thickness = 1f)
    {
        if (points.Length <= 1)
            return;

        for (int i = 0; i < points.Length; i ++)
        {
            if (points.Length > i + 1)
                Line(points[i], points[i + 1], color, thickness);
        }
    }

    #endregion

    #region Draws a Dashed Line

    /// <summary>
    ///     Draws a dashed line.
    /// </summary>
    /// <param name="from">Start point.</param>
    /// <param name="to">Target point.</param>
    /// <param name="color">Color of the line.</param>
    /// <param name="thickness">Thickness of the line.</param>
    /// <param name="length">Length of the dash.</param>
    /// <param name="offset">Offset of the dash, in percent.</param>
    public void DashedLine(Vector2 from, Vector2 to, Color color, float thickness = 1f, float length = 4f, float offset = 0f)
    {
        var difference     = to - from;
        var distance      = difference.Length();
        var axis          = Vector2.Normalize(difference);
        var perpendicular = new Vector2(axis.Y, -axis.X) * (thickness * 0.5f);

        offset = ((offset % 1f) + 1f) % 1f;

        var startD = length * offset * 2f;
        if (startD > length)
            startD -= length * 2f;

        for (float d = startD; d < distance; d += length * 2f)
        {
            var a = from + axis * Math.Max(d, 0f);
            var b = from + axis * Math.Min(d + length, distance);

            Quad(
                a + perpendicular,
                b + perpendicular,
                b - perpendicular,
                a - perpendicular,
                color
            );
        }
    }

    #endregion

    #region Draws a Triangle
    
    /// <summary>
    ///     Draws a triangle.
    /// </summary>
    /// <param name="v0">The first point of the triangle.</param>
    /// <param name="v1">The second point of the triangle.</param>
    /// <param name="v2">The third point of the triangle.</param>
    /// <param name="color">Color of the triangle.</param>
    public void Triangle(Vector2 v0, Vector2 v1, Vector2 v2, Color color)
    {
        _batch.IndexCount += 3;

        _pixels[0].Position = Matrix.MultiplyVector(v0);
        _pixels[1].Position = Matrix.MultiplyVector(v1);
        _pixels[2].Position = Matrix.MultiplyVector(v2);

        _pixels[0].Color = 
        _pixels[1].Color = 
        _pixels[2].Color = color;

        _pixels[0].Mult = 
        _pixels[1].Mult = 
        _pixels[2].Mult = 0;

        _pixels[0].Wash = 
        _pixels[1].Wash = 
        _pixels[2].Wash = 0;

        _pixels[0].Fill = 
        _pixels[1].Fill = 
        _pixels[2].Fill = 255;

        _mesh.AddTriangle(ref _pixels[0], ref _pixels[1], ref _pixels[2]);
    }

    #endregion
    
    #region Draws a Rectangle

    /// <summary>
    ///     Draws a rectangle.
    /// </summary>
    /// <param name="x">The horizontal position of the rectangle.</param>
    /// <param name="y">The vertical position of the rectangle.</param>
    /// <param name="width">The Width of the rectangle.</param>
    /// <param name="height">The Height of the rectangle.</param>
    /// <param name="color">The color of the rectangle.</param>
    public void Rectangle(float x, float y, float width, float height, Color color)
    {
        Quad(
            new Vector2(x,         y         ),
            new Vector2(x + width, y         ),
            new Vector2(x + width, y + height),
            new Vector2(x,         y + height),
            color
        );
    }

    /// <summary>
    ///     Draws a rectangle.
    /// </summary>
    /// <param name="rect">The rectangle to draw.</param>
    /// <param name="color">The color of the rectangle.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Rectangle(Rectangle rect, Color color)
    {
        Rectangle(rect.X, rect.Y, rect.Width, rect.Height, color);
    }

    /// <summary>
    ///     Draws a rectangle.
    /// </summary>
    /// <param name="rect">The rectangle to draw.</param>
    /// <param name="color">The color of the rectangle.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Rectangle(RectangleI rect, Color color)
    {
        Rectangle(rect.X, rect.Y, rect.Width, rect.Height, color);
    }

    #endregion

    #region Draws a Dashed Rectangle

    /// <summary>
    ///     Draws a dashed rectangle.
    /// </summary>
    /// <param name="x">The horizontal position of the rectangle.</param>
    /// <param name="y">The vertical position of the rectangle.</param>
    /// <param name="width">The Width of the rectangle.</param>
    /// <param name="height">The Height of the rectangle.</param>
    /// <param name="color">The color of the rectangle.</param>
    public void DashedRectangle(float x, float y, float width, float height, Color color, float length = 4, float offset = 0)
    {
        DashedLine(new Vector2(x,         y         ), new Vector2(x + width, y         ), color, 1, length, offset);
        DashedLine(new Vector2(x,         y         ), new Vector2(x,         y + height), color, 1, length, offset);
        DashedLine(new Vector2(x + width, y         ), new Vector2(x + width, y + height), color, 1, length, offset);
        DashedLine(new Vector2(x,         y + height), new Vector2(x + width, y + height), color, 1, length, offset);
    }

    /// <summary>
    ///     Draws a dashed rectangle.
    /// </summary>
    /// <param name="rect">The rectangle to draw.</param>
    /// <param name="color">The color of the rectangle.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void DashedRectangle(Rectangle rect, Color color, float length = 4, float offset = 0)
    {
        DashedRectangle(rect.X, rect.Y, rect.Width, rect.Height, color, length, offset);
    }

    /// <summary>
    ///     Draws a dashed rectangle.
    /// </summary>
    /// <param name="rect">The rectangle to draw.</param>
    /// <param name="color">The color of the rectangle.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void DashedRectangle(RectangleI rect, Color color, float length = 4, float offset = 0)
    {
        DashedRectangle(rect.X, rect.Y, rect.Width, rect.Height, color, length, offset);
    }
    
    #endregion

    #region Draws a Hollow Rectangle

    /// <summary>
    ///     Draws a hollow rectangle.
    /// </summary>
    /// <param name="x">The horizontal position of the rectangle.</param>
    /// <param name="y">The vertical position of the rectangle.</param>
    /// <param name="width">The Width of the rectangle.</param>
    /// <param name="height">The Height of the rectangle.</param>
    /// <param name="color">The color of the rectangle.</param>
    public void HollowRectangle(float x, float y, float width, float height, Color color)
    {
        Line(new Vector2(x,         y         ), new Vector2(x + width, y         ), color);
        Line(new Vector2(x,         y         ), new Vector2(x,         y + height), color);
        Line(new Vector2(x + width, y + height), new Vector2(x + width, y         ), color);
        Line(new Vector2(x + width, y + height), new Vector2(x,         y + height), color);
    }

    /// <summary>
    ///     Draws a hollow rectangle.
    /// </summary>
    /// <param name="rect">The rectangle to draw.</param>
    /// <param name="color">The color of the rectangle.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void HollowRectangle(Rectangle rect, Color color)
    {
        HollowRectangle(rect.X, rect.Y, rect.Width, rect.Height, color);
    }

    /// <summary>
    ///     Draws a hollow rectangle.
    /// </summary>
    /// <param name="rect">The rectangle to draw.</param>
    /// <param name="color">The color of the rectangle.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void HollowRectangle(RectangleI rect, Color color)
    {
        HollowRectangle(rect.X, rect.Y, rect.Width, rect.Height, color);
    }

    #endregion

    #region Draws a Circle
    
    /// <summary>
    ///     Draws a circle.
    /// </summary>
    /// <param name="x">Horizontal position of the circle.</param>
    /// <param name="y">Vertical position of the circle.</param>
    /// <param name="radius">Radius of the circle.</param>
    /// <param name="color">Color to be used.</param>
    /// <param name="segments">Number of steps used for drawing the circle.</param>
    public void Circle(float x, float y, float radius, Color color, int segments = 32)
    {
        if (segments < 3)
            segments = 3;

        if (_mesh == null)
            return;
        
		var vertexCount = (uint)_mesh.VertexCount;
		var indexCount  = _mesh.IndexCount;
		var num1 = (float)Math.PI * 2f / (float)segments;
		var num2 = num1;
		var num3 = vertexCount + 2;
		var num4 = vertexCount + 1;

        _pixels[0].Mult = 0;
        _pixels[0].Wash = 0;
        _pixels[0].Fill = 255;

		_pixels[0].Color = color;
		_pixels[0].Position.X = x;
		_pixels[0].Position.Y = y;
        _pixels[0].Position = Matrix.MultiplyVector(_pixels[0].Position);

		_mesh?.AddVertex(_pixels[0]);

		_pixels[0].Position.X += radius;
		_mesh?.AddVertex(_pixels[0]);

		for (int i = 1; i < segments; i++)
		{
			_pixels[0].Position.X = x + (float)Math.Cos(num2) * radius;
			_pixels[0].Position.Y = y + (float)Math.Sin(num2) * radius;
            _pixels[0].Position = Matrix.MultiplyVector(_pixels[0].Position);

			_mesh?.AddVertex(_pixels[0]);
			_mesh?.AddIndices(vertexCount, num4 ++, num3 ++);

			num2 += num1;
		}

        if (_mesh != null)
        {
    		_mesh.AddIndices(vertexCount, num4, vertexCount + 1);
            _batch.IndexCount += _mesh.IndexCount - indexCount;
        }
    }

    #endregion
    
    #region Draws a Texture
    
    /// <summary>
    ///     Draws a <see cref="Battery.Texture"/> at the given position.
    /// </summary>
    /// <param name="texture">The texture to draw.</param>
    /// <param name="position">The position of the texture.</param>
    /// <param name="blend">The color used to blend the texture.</param>
    /// <param name="washed">Whether the texture will be washed by the blending color.</param>
    public void Texture(Texture texture, Vector2 position, Color? blend = null, bool washed = false)
    {
        Texture(
            texture,
            position + new Vector2(0,             0             ),
            position + new Vector2(texture.Width, 0             ),
            position + new Vector2(texture.Width, texture.Height),
            position + new Vector2(0,             texture.Height),
            Vector2.Zero,
            Vector2.UnitX,
            Vector2.One,
            Vector2.UnitY,
            blend ?? Color.White,
            washed
        );
    }

    /// <summary>
    ///     Draws a <see cref="Battery.Texture"/> at the given position using a rectangle to cut out part of it.
    /// </summary>
    /// <param name="texture">The texture to draw.</param>
    /// <param name="quad">The rectangle used to clip the texture.</param>
    /// <param name="position">The position of the texture.</param>
    /// <param name="blend">The color used to blend the texture.</param>
    /// <param name="washed">Whether the texture will be washed by the blending color.</param>
    public void Texture(Texture texture, Rectangle quad, Vector2 position, Color? blend = null, bool washed = false)
    {
        var tx0 = quad.X      / texture.Width;
        var ty0 = quad.Y      / texture.Height;
        var tx1 = quad.Right  / texture.Width;
        var ty1 = quad.Bottom / texture.Height;

        // Draws the texture.
        Texture(
            texture,
            position + new Vector2(0,             0             ),
            position + new Vector2(texture.Width, 0             ),
            position + new Vector2(texture.Width, texture.Height),
            position + new Vector2(0,             texture.Height),
            new Vector2(tx0, ty0),
            new Vector2(tx1, ty0),
            new Vector2(tx1, ty1),
            new Vector2(tx0, ty1), 
            blend ?? Color.White,
            washed
        );
    }

    /// <summary>
    ///     Draws a <see cref="Battery.Texture"/> at the given position with the given origin, scale and rotation angle.
    /// </summary>
    /// <param name="texture">The texture to draw.</param>
    /// <param name="position">The position of the texture.</param>
    /// <param name="scale">The scale of the texture.</param>
    /// <param name="origin">The origin of the texture.</param>
    /// <param name="angle">The rotation angle of the texture.</param>
    /// <param name="blend">The color used to blend the texture.</param>
    /// <param name="washed">Whether the texture will be washed by the blending color.</param>
    public void Texture(Texture texture, Vector2 position, Vector2 scale, Vector2 origin, float angle, Color? blend = null, bool washed = false)
    {
        var prevMatrix = Matrix;

        // Sets the current matrix.
        Matrix = Matrix3x2.Identity.Transform(position, origin, scale, angle) * Matrix;
        
        // Draws the texture.
        Texture(
            texture,
            new Vector2(0,             0             ),
            new Vector2(texture.Width, 0             ),
            new Vector2(texture.Width, texture.Height),
            new Vector2(0,             texture.Height),
            Vector2.Zero,
            Vector2.UnitX,
            Vector2.One,
            Vector2.UnitY,
            blend ?? Color.White,
            washed
        );

        Matrix = prevMatrix;
    }
    

    /// <summary>
    ///     Draws a <see cref="Battery.Texture"/> at the given position with the given origin, scale and rotation angle using a rectangle to cut out part of it.
    /// </summary>
    /// <param name="texture">The texture to draw.</param>
    /// <param name="quad">The rectangle used to clip the texture.</param>
    /// <param name="position">The position of the texture.</param>
    /// <param name="scale">The scale of the texture.</param>
    /// <param name="origin">The origin of the texture.</param>
    /// <param name="angle">The rotation angle of the texture.</param>
    /// <param name="blend">The color used to blend the texture.</param>
    /// <param name="washed">Whether the texture will be washed by the blending color.</param>
    public void Texture(Texture texture, Rectangle quad, Vector2 position, Vector2 scale, Vector2 origin, float angle, Color? blend = null, bool washed = false)
    {
        var prevMatrix = Matrix;

        // Sets the current matrix.
        Matrix = Matrix3x2.Identity.Transform(position, origin, scale, angle) * Matrix;

        // Gets the quad texture coordinates.
        var tx0 = quad.X      / texture.Width;
        var ty0 = quad.Y      / texture.Height;
        var tx1 = quad.Right  / texture.Width;
        var ty1 = quad.Bottom / texture.Height;

        // Draws the texture.
        Texture(
            texture,
            new Vector2(0,          0          ),
            new Vector2(quad.Width, 0          ),
            new Vector2(quad.Width, quad.Height),
            new Vector2(0,          quad.Height),
            new Vector2(tx0, ty0),
            new Vector2(tx1, ty0),
            new Vector2(tx1, ty1),
            new Vector2(tx0, ty1), 
            blend ?? Color.White,
            washed
        );

        Matrix = prevMatrix;
    }

    /// <summary>
    ///     Draws a <see cref="Battery.Texture"/> by using the given vertex positions and texture coordinates.
    /// </summary>
    /// <param name="texture">The texture to draw.</param>
    /// <param name="v0">The position of the first vertex.</param>
    /// <param name="v1">The position of the second vertex.</param>
    /// <param name="v2">The position of the third vertex.</param>
    /// <param name="v3">The position of the fourth vertex.</param>
    /// <param name="a">The texture coordinate of the first vertex.</param>
    /// <param name="b">The texture coordinate of the second vertex.</param>
    /// <param name="c">The texture coordinate of the third vertex.</param>
    /// <param name="d">The texture coordinate of the fourth vertex.</param>
    /// <param name="blend">The color used to blend the texture.</param>
    /// <param name="washed">Whether the texture will be washed by the blending color.</param>
    public void Texture(Texture texture, Vector2 v0, Vector2 v1, Vector2 v2, Vector2 v3, Vector2 a, Vector2 b, Vector2 c, Vector2 d, Color? blend = null, bool washed = false)
    {
        // Flips horizontally the texture.
        if (texture.FlipX)
        {
            a.X = 1 - a.X;
            b.X = 1 - b.X;
            c.X = 1 - c.X;
            d.X = 1 - d.X;
        }

        // Flips vertically the texture.
        if (texture.FlipY)
        {   
            a.Y = 1 - a.Y;
            b.Y = 1 - b.Y;
            c.Y = 1 - c.Y;
            d.Y = 1 - d.Y;
        }

        SetTexture(texture);
        Quad(v0, v1, v2, v3, a, b, c, d, blend ?? Color.White, washed);
    }

    #endregion

    #region Draws a Sine Wave

    /// <summary>
    ///     Draws a sine wave.
    /// </summary>
    /// <param name="x">The horizontal offset of the wave.</param>
    /// <param name="y">The vertical offset of the wave.</param>
    /// <param name="width">The Width of the wave.</param>
    /// <param name="height">The Height of the wave.</param>
    /// <param name="color">The color of the wave.</param>
    /// <param name="frequency">The frequency of the wave.</param>
    /// <param name="steps">The number of steps</param>
    public void SineWave(float x, float y, float width, float height, Color color, float offset = 0f, float frequency = 0.5f, int steps = 64)
    {
        var position = new Vector2(x, y);
        for (int i = 0; i < steps; i ++)
        {
            var xx  = i * (width/steps);
            var yy1 = height * MathF.Sin( i      * frequency + offset);
            var yy2 = height * MathF.Sin((i + 1) * frequency + offset);

            Line(
                position + new Vector2(xx,                              yy1),
                position + new Vector2(xx + MathF.Ceiling(width/steps), yy2),
                color
            );
        }
    }

    #endregion


    #region Render Target operations

    /// <summary>
    ///     Sets the the current Render Target.
    /// </summary>
    /// <param name="target">The target to set.</param>
    /// <param name="matrix">The optional view matrix.</param>
    public void PushTarget(RenderTarget target, Matrix3x2? matrix = null)
    {
        _targetStack.Push(_batch.Target);
        SetRenderTarget(target);
        PushMatrix(matrix ?? Matrix3x2.Identity);
    }

    /// <summary>
    ///     Resets the last render target.
    /// </summary>
    public void PopTarget()
    {
        if (_targetStack.Count == 0)
            throw new Exception("Unable to pop the target because the RenderTarget Stack is empty.");

        SetRenderTarget(_targetStack.Pop());
        PopMatrix();
    }

    #endregion
    
    #region Matrix operations

    /// <summary>
    ///     Transforms the current matrix by using the specified matrix.
    /// </summary>
    /// <param name="matrix">The matrix used to transform.</param>
    public void PushMatrix(Matrix3x2 matrix)
    {
        _matrixStack.Push(Matrix);
        Matrix = matrix;
    }

    /// <summary>
    ///     Pop the lsat matrix.
    /// </summary>
    public void PopMatrix()
    {
        Matrix = _matrixStack.Pop();
    }

    #endregion
}