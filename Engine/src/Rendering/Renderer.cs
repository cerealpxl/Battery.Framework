using Battery.Framework;

namespace Battery.Engine;

/// <summary>
///     Class that extends the renderer components.
/// </summary>
public class Renderer : Component, IRenderer
{
    /// <inheritdoc/>
    public int Depth { get; set; } = 0;

    /// <inheritdoc/>
    public bool Visible { get; set; } = true;
    
    /// <inheritdoc/>
    public Tag Tag { get; set; } = Tag.Default;

    /// <summary>
    ///     The default graphics batch.
    /// </summary>
    public Batch? Batch => Game.Instance == null ? null : Game.Instance.Graphics.Batch;

    /// <summary>
    ///     Creates a new renderer.
    /// </summary>
    public Renderer()
        : this(Tag.Default, true)
    {
    }

    /// <summary>
    ///     Creates a new renderer.
    /// </summary>
    /// <param name="tags">The tags of the renderer.</param>
    /// <param name="visible">Whether the component starts visible.</param>
    public Renderer(Tag tags, bool visible = true)
    {
        Tag     = tags;
        Visible = visible;
    }

    /// <inheritdoc/>
    public virtual void Render()
    {
    }
}