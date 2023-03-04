namespace Battery.Framework;

/// <summary>
///     Structure that describes a single render call to the <see cref="GameGraphics" />.
/// </summary>
public struct RenderPass<T> where T : struct, IVertex
{
    /// <summary>
    ///     The surface to use.
    /// </summary>
    public Surface? Surface;
    
    /// <summary>
    ///     The viewport 
    /// </summary>
    public RectangleI? Viewport;

    /// <summary>
    ///     The material to use.
    /// </summary>
    public ShaderMaterial Material;

    /// <summary>
    ///     The mesh to use.
    /// </summary>
    public Mesh<T> Mesh;

    /// <summary>
    ///     The first index of the mesh to render.
    /// </summary>
    public int IndexStart;

    /// <summary>
    ///     The number of of the mesh indices to render.
    /// </summary>
    public int IndexCount;

    /// <summary>
    ///     The color used to clear the current surface.
    /// </summary>
    public Color? ClearColor;

    /// <summary>
    ///     Creates a new instance of the <see cref="RenderPass" /> class.
    /// </summary>
    public RenderPass(Mesh<T> mesh, ShaderMaterial material, Surface? surface = null, Color? clearColor = null)
    {
        Surface    = surface;
        Material   = material;
        Mesh       = mesh;
        IndexStart = 0;
        IndexCount = mesh.IndexCount;
        Viewport   = null;
        ClearColor = clearColor;
    }
}