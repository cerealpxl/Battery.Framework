namespace Battery.Framework;

/// <summary>
///     Structure that describes a single render call to the <see cref="GameGraphics" />.
/// </summary>
public struct RenderPass<T> where T : struct, IVertex
{
    /// <summary>
    ///     The target to use.
    /// </summary>
    public RenderTarget Target;
    
    /// <summary>
    ///     The viewport 
    /// </summary>
    public RectangleI? Viewport;

    /// <summary>
    ///     The mesh to use.
    /// </summary>
    public Mesh<T> Mesh;

    /// <summary>
    ///     The material to use.
    /// </summary>
    public ShaderMaterial Material;

    /// <summary>
    ///     The first index of the mesh to render.
    /// </summary>
    public int IndexStart;

    /// <summary>
    ///     The number of of the mesh indices to render.
    /// </summary>
    public int IndexCount;

    /// <summary>
    ///     Creates a new instance of the <see cref="RenderPass" /> class.
    /// </summary>
    /// <param name="target">The target to use.</param>
    /// <param name="mesh">The mesh to use.</param>
    /// <param name="material">The material to use.</param>
    public RenderPass(RenderTarget target, Mesh<T> mesh, ShaderMaterial material)
    {
        Target     = target;
        Viewport   = null;
        Mesh       = mesh;
        Material   = material;
        IndexStart = 0;
        IndexCount = mesh.IndexCount;
    }
}