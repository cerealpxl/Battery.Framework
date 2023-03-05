using System.Numerics;

namespace Battery.Framework;

/// <summary>
///     The class that extends all Game Platforms.
///     Provides the window handle logic.
/// </summary>
public abstract class GamePlatform : RenderTarget
{
    /// <summary>
    ///     The game to which the platform belongs to.
    /// </summary>
    public Game? Game { get; protected set; }

    /// <summary>
    ///     The title of the window.
    /// </summary>
    public abstract string Title { get; set; }

    /// <summary>
    ///     The dimensions of the window.
    /// </summary>
    public abstract Vector2 Dimensions { get; set; }

    /// <summary>
    ///     The width of the window.
    /// </summary>
    public override int Width => (int)Dimensions.X;

    /// <summary>
    ///     The height of the window.
    /// </summary>
    public override int Height => (int)Dimensions.Y;

    /// <summary>
    ///     Gets or Sets whether the window is in the fullscreen mode.
    /// </summary>
    public abstract bool Fullscreen { get; set; }

    /// <summary>
    ///     Gets or Sets whether the window has a border.
    /// </summary>
    public abstract bool Borderless { get; set; }

    /// <summary>
    ///     Gets or Sets whether the vertical synchronization is enabled.
    /// </summary>
    public abstract bool VSync { get; set; }

    /// <summary>
    ///     Gets or Sets whether the window is currently visible to the user.
    /// </summary>
    public abstract bool Visible { get; set; }

    /// <summary>
    ///     Gets whether the window is currently focused.
    /// </summary>
    public abstract bool Focused { get; }

    /// <summary>
    ///     Action executed when the window presents the draw data.
    /// </summary>
    public Action? OnRender;

    /// <summary>
    ///     Action executed when the window gaim focus.
    /// </summary>
    public Action? OnFocusGained;

    /// <summary>
    ///     Action executed when the window lose focus.
    /// </summary>
    public Action? OnFocusLost;

    /// <summary>
    ///     Action executed immediately when the close event is requested.
    /// </summary>
    public Action? OnExit;

    /// <summary>
    ///     Begins the Window Platform.
    /// </summary>
    /// <param name="game">The game instance to witch the platform belongs to.</param>
    /// <param name="title">The title of the window.</param>
    /// <param name="width">The width of the window.</param>
    /// <param name="height">The height of the window.</param>
    /// <param name="fullscreen">Whether the window starts in the fullscreen mode.</param>
    /// <param name="borderless">Whether the window starts without a border.</param>
    /// <param name="vsync">Whether the window starts with the vertical synchronization enabled.</param>
    public abstract void Begin(Game game, string title, int width, int height, bool fullscreen = false, bool borderless = false, bool vsync = false);

    /// <summary>
    ///     Ends the Window Platform.
    /// </summary>
    public abstract void End();

    /// <summary>
    ///     Updates the window platform.
    /// </summary>
    public abstract void Update();

    /// <summary>
    ///     Presents the Draw Data to the window.
    /// </summary>
    public abstract void Present();
}