using System.Numerics;
using System.Runtime.InteropServices;

namespace Battery.Framework;

/// <summary>
///     A 2D batch that draw shapes and images.
/// </summary>
public class Batch 
{
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
    ///     Default vertex used for rendering to the screen.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vertex : IVertex
    {
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
}
