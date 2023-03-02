namespace Battery.Framework;

/// <summary>
///     Battery Game.
///     Provides the Graphics and Window initialization and runs the Game Loop.
/// </summary>
public class Game
{
    /// <summary>
    ///     Whether the game is running.
    /// </summary>
    public static bool Running { get; private set; }

    /// <summary>
    ///     Whether the game is exiting. 
    ///     If true, the game ends at the end of the current frame.
    /// </summary>
    public static bool Exiting { get; private set; }

    /// <summary>
    ///     Whether the game updates while the window is unfocused.
    /// </summary>
    public bool RunWhileUnfocused = false;

    /// <summary>
    ///     The <see cref="GamePlatform" /> used by the game.
    /// </summary>
    public GamePlatform Platform;

    /// <summary>
    ///     The <see cref="GameGraphics" /> used by the game.
    /// </summary>
    public GameGraphics Graphics;

    /// <summary>
    ///     Action called before the game starts.
    /// </summary>
    public event Action? OnBegin;

    /// <summary>
    ///     Action called to update the game.
    /// </summary>
    public event Action? OnUpdate;

    /// <summary>
    ///     Action called to render the game.
    /// </summary>
    public event Action? OnRender;

    /// <summary>
    ///     Action called after the game ends.
    /// </summary>
    public event Action? OnEnd;

    /// <summary>
    ///     Creates a new instance of the Game class.
    /// </summary>
    /// <param name="platform">The optional <see cref="GamePlatform" /> to handle the game window.</param>
    /// <param name="graphics">The optional <see cref="GameGraphics" /> to handle the graphics backend.</param>
    public Game(GamePlatform? platform = null, GameGraphics? graphics = null)
    {
        Platform = platform ?? GamePlatform.CreateDefault(this);
        Graphics = graphics ?? GameGraphics.CreateDefault(this);
    }

    /// <summary>
    ///     Begins the Platform and Graphics and runs the Game Loop.
    /// </summary>
    public void Launch(string title, int width, int height)
    {
        Platform.Begin(title, width, height);
        Graphics.Begin();
        OnBegin?.Invoke();

        // Begins the Game Loop.
        Running = true;
        Exiting = false;

        while (Running)
        {
            Platform.Update();

            if (Platform.Focused || RunWhileUnfocused)
            {
                OnUpdate?.Invoke();
            }

            if (Exiting)
                Running = false;

            if (Running)
            {
                OnRender?.Invoke();
                Platform.Present();
            }

            // It avoids from the high CPU Consomption.
            Thread.Sleep(0_001);
        }

        Running = false;
        Exiting = false;

        OnEnd?.Invoke();
        Graphics.End();
        Platform.End();
    }

    /// <summary>
    ///     Request the Game Exit.
    /// </summary>
    public void Exit()
        => Exiting = true;
}