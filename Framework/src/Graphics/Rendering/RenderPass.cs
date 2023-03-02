namespace Battery.Framework;

/// <summary>
///     Structure that describes a single render call to the <see cref="GameGraphics" />.
/// </summary>
public struct RenderPass
{
    /// <summary>
    ///     The first index to render.
    /// </summary>
    public int IndexStart;

    /// <summary>
    ///     The number of indices to render.
    /// </summary>
    public int IndexCount;

    /// <summary>
    ///     The material to use.
    /// </summary>
    public ShaderMaterial Material;

    /// <summary>
    ///     The mesh to use.
    /// </summary>
    public Mesh Mesh;

    /// <summary>
    ///     The color used to clear the current framebuffer.
    /// </summary>
    public Color ClearColor = Color.Black;

    /// <summary>
    ///     Creates a new instance of the <see cref="RenderPass" /> class.
    /// </summary>
    public RenderPass(Mesh mesh, ShaderMaterial material)
    {
        Mesh       = mesh;
        Material   = material;
        IndexStart = 0;
        IndexCount = mesh.IndexCount;
    }
}