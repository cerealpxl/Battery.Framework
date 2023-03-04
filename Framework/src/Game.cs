using System.Diagnostics;

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
    ///     The struct that store the current time data.
    /// </summary>
    public GameTime Time = new GameTime();

    /// <summary>
    ///     Action called before the game starts.
    /// </summary>
    public event Action? OnBegin;

    /// <summary>
    ///     Action called to update the game.
    /// </summary>
    public event Action<GameTime>? OnUpdate;

    /// <summary>
    ///     Action called to render the game.
    /// </summary>
    public event Action<GameTime>? OnRender;

    /// <summary>
    ///     Action called after the game ends.
    /// </summary>
    public event Action? OnEnd;

    /// <summary>
    ///     Creates a new instance of the Game class.
    /// </summary>
    /// <param name="platform">The <see cref="GamePlatform" /> to handle the game window.</param>
    /// <param name="graphics">The <see cref="GameGraphics" /> to handle the graphics backend.</param>
    public Game(GamePlatform platform, GameGraphics? graphics = null)
    {
        Platform = platform;
        Graphics = graphics ?? GameGraphics.CreateDefault(this);
    }

    /// <summary>
    ///     Begins the Platform and Graphics and runs the Game Loop.
    /// </summary>
    public void Launch(string title, int width, int height, bool fullscreen = false, bool borderless = false, bool vsync = false)
    {
        Platform.Begin(this, title, width, height, fullscreen, borderless, vsync);
        Graphics.Begin();
        OnBegin?.Invoke();

        // Begins the Game Loop.
        Running = true;
        Exiting = false;

        // Assign time variables.
        var stopwatch   = Stopwatch.StartNew();
        var accumulator = 0.0;

        while (Running)
        {
            Platform.Update();
            Time.Update(stopwatch.Elapsed, Time.Elapsed);

            // Snaps the delta time to a nice framerate.
            if (Math.Abs(Time.RawDelta - 1f / 120f) < 0.0002f) Time.RawDelta = 1f / 120f;
            if (Math.Abs(Time.RawDelta - 1f / 60f)  < 0.0002f) Time.RawDelta = 1f / 60f;
            if (Math.Abs(Time.RawDelta - 1f / 30f)  < 0.0002f) Time.RawDelta = 1f / 30f;
            if (Math.Abs(Time.RawDelta - 1f / 15f)  < 0.0002f) Time.RawDelta = 1f / 15f;

            // Increase the accumulator.
            accumulator += Time.RawDelta;

            // Prevents unexpected crashes.
            if (accumulator >= Time.FixedDelta * 8f) 
            {
                accumulator   = 0f;
                Time.RawDelta = Time.FixedDelta;
            }

            // Perform an update when the frame accumulator reaches the fixed delta tine.
            var delta = Time.RawDelta;
            while (accumulator >= Time.FixedDelta && !Exiting)
            {
                accumulator  -= Time.FixedDelta;
                Time.RawDelta = Time.FixedDelta;
                
                if (Platform.Focused || RunWhileUnfocused)
                    OnUpdate?.Invoke(Time);

                Input.Update();
            }

            // Updates the time variables for the variable timestep.
            Time.RawDelta = (float)delta;

            if (Exiting)
                Running = false;

            // Renders the game.
            if (Running)
            {
                OnRender?.Invoke(Time);
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