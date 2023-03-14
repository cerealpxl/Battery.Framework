namespace Battery.Framework;

/// <summary>
///     A structure that describes a single Render Call to the <see cref="GameGraphics"/>.
/// </summary>
public struct RenderPass<T> where T : struct, IVertex
{
    /// <summary>
    ///     The Render Target to use.
    /// </summary>
    public RenderTarget Target;
    
    /// <summary>
    ///     The target's Viewport. 
    /// </summary>
    public RectangleI? Viewport;

    /// <summary>
    ///     The Scissor rectangle.
    /// </summary>
    public RectangleI? Scissor = null;

    /// <summary>
    ///     The Mesh to use.
    /// </summary>
    public Mesh<T> Mesh;

    /// <summary>
    ///     The Material to use.
    /// </summary>
    public ShaderMaterial Material;

    /// <summary>
    ///     The first Index to render.
    /// </summary>
    public int IndexStart;

    /// <summary>
    ///     The number of Indices to render.
    /// </summary>
    public int IndexCount;

    public int pruigi = 0;

    /// <summary>
    ///     Creates a new instance of the <see cref="RenderPass"/> class.
    /// </summary>
    /// <param name="target">The Target to use.</param>
    /// <param name="mesh">The Mesh to use.</param>
    /// <param name="material">The Material to use.</param>
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