using Battery.Framework;

namespace Battery.Engine;

/// <summary>
///     Represents a Renderer Component.
///     Provides the Render methods.
/// </summary>
public interface IRenderer : ITagged
{
    /// <summary>
    ///     The depth of the component.
    /// </summary>
    int Depth { get; set; }

    /// <summary>
    ///     Whether the component is visible.
    /// </summary>
    bool Visible { get; set; }

    /// <summary>
    ///     Called before the Render method.
    /// </summary>
    /// <param name="time">The <see cref="GameTime" /> struct.</param>
    abstract void RenderBegin(GameTime time);

    /// <summary>
    ///     Called to render the component to the screen.
    /// </summary>
    /// <param name="time">The <see cref="GameTime" /> struct.</param>
    abstract void Render(GameTime time);

    /// <summary>
    ///     Called after the Render method.
    /// </summary>
    /// <param name="time">The <see cref="GameTime" /> struct.</param>
    abstract void RenderEnd(GameTime time);
}

/// <summary>
///     IRenderer method extensions.
/// </summary>
public static class IRendererExt
{
    /// <summary>
    ///     Show this renderer.
    /// </summary>
    public static IRenderer Show(this IRenderer renderer)
    {
        renderer.Visible = true;
        return renderer;
    }

    /// <summary>
    ///     Hide this renderer.
    /// </summary>
    public static IRenderer Hide(this IRenderer renderer)
    {
        renderer.Visible = false;
        return renderer;
    }
}