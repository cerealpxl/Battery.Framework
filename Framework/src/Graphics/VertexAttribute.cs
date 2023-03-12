namespace Battery.Framework;

/// <summary>
///     Describes a single Vertex Attribute.
/// </summary>
public struct VertexAttribute
{
    /// <summary>
    ///     The name of the Attribute.
    /// </summary>
    public readonly string Name;

    /// <summary>
    ///     The type of the Attribute.
    /// </summary>
    public readonly VertexAttributeType Type;

    /// <summary>
    ///     The number of components in the Attribute.
    /// </summary>
    public readonly int ComponentCount;

    /// <summary>
    ///     The size of a single component in the Attribute.
    /// </summary>
    public readonly int ComponentSize;

    /// <summary>
    ///     Whether the attribute should be normalized.
    /// </summary>
    public readonly bool Normalized;

    /// <summary>
    ///     Creates a new instance of the <see cref="VertexAttribute" /> struct.
    /// </summary>
    /// <param name="name">The name of the Attribute.</param>
    /// <param name="type">The type of the Attribute.</param>
    /// <param name="normalized">Whether the attribute should be normalized.</param>
    public VertexAttribute(string name, VertexAttributeType type, int count, bool normalized = false)
    {
        Name       = name;
        Type       = type;
        Normalized = normalized;
        
        ComponentCount = count;
        ComponentSize  = type switch
        {
            VertexAttributeType.Byte  => sizeof(byte),
            VertexAttributeType.Int   => sizeof(int),
            VertexAttributeType.Short => sizeof(short),
            VertexAttributeType.Float => sizeof(float),

            _ => throw new NotImplementedException(),
        };
    }
}