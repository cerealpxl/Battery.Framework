using Battery.Framework;

namespace Battery.Engine;

/// <summary>
///     Provides the component rendering logic.
/// </summary>
public class RenderSystem : ComponentSystem
{
    /// <summary>
    ///     The table that stores all the renderers of the system.
    /// </summary>
    public Table<IRenderer> Renderers = new Table<IRenderer>();

    /// <summary>
    ///     Creates a new Render System.
    /// </summary>
    public RenderSystem()
        : base(false, true)
    {
    }

    /// <summary>
    ///     Renders all the renderable components to the enabled cameras.
    /// </summary>
    public override void Render()
    {
        if (!(Game.Instance is Game game))
            return;

        // Draws the renderers to the Batcher.
        game.Graphics.Batch?.Clear();

        foreach (var renderer in Renderers)
        {
            if (renderer.Visible)
                renderer.Render();
        }

        game.Graphics.Batch?.Present();
    }

    /// <inheritdoc/>
    public override void Add(Component component)
    {
        if (component is IRenderer renderer)
            Renderers.Add(renderer);
    }

    /// <inheritdoc/>
    public override void Remove(Component component)
    {
        if (component is IRenderer renderer)
            Renderers.Remove(renderer);
    }
}