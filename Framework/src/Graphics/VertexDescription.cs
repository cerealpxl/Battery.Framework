namespace Battery.Framework;

/// <summary>
///     Describe the vertex attributes to the <see cref="Shader" />.
/// </summary>
public class VertexDescription
{
    /// <summary>
    ///     The list storing the vertex attributes.
    /// </summary>
    public readonly List<VertexAttribute> Attributes = new List<VertexAttribute>();

    /// <summary>
    ///     The total size of the vertex.
    /// </summary>
    public readonly int Stride;

    /// <summary>
    ///     Creates a new instance of the <see cref="VertexDescription" /> class.
    /// </summary>
    /// <param name="attributes">The attributes to describe.</param>
    public VertexDescription(params VertexAttribute[] attributes)
    {
        Stride = 0;

        foreach (var attribute in attributes)
        {
            Stride += attribute.ComponentCount * attribute.ComponentSize;
            Attributes.Add(attribute);
        }
    }
}