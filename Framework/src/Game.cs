namespace Battery.Framework;

/// <summary>
///     Battery Game.
///     Provides the Graphics and Window initialization and runs the Game Loop.
/// </summary>
public class Game
{
    /// <summary>
    ///     The current game instance.
    /// </summary>
    public static Game? Instance { get; private set; }

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
    ///     The list that store all the managers of the game.
    /// </summary>
    public Table<GameManager> Managers = new Table<GameManager>();

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
    /// <param name="platform">The <see cref="GamePlatform" /> to handle the game window.</param>
    /// <param name="graphics">The <see cref="GameGraphics" /> to handle the graphics backend.</param>
    public Game(GamePlatform platform, GameGraphics? graphics = null)
    {
        Instance = this;
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
        Begin();

        // Begins the Game Loop.
        Running = true;
        Exiting = false;

        // Assign time variables.
        var accumulator = 0.0;

        // We don't want the time of the Begin method.
        Time.Update();

        while (Running)
        {
            Platform.Update();
            Time.Update();

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
                    Update();

                Keyboard.Update();
                Mouse.Update();
            }

            // Updates the time variables for the variable timestep.
            Time.RawDelta = (float)delta;

            if (Exiting)
                Running = false;

            // Renders the game.
            if (Running)
            {
                Render();
                Platform.Present();
            }

            // It avoids from the high CPU Consomption.
            Thread.Sleep(0_001);
        }

        Running = false;
        Exiting = false;

        End();
        Graphics.End();
        Platform.End();
    }

    /// <summary>
    ///     Request the Game Exit, this will not close immediately and will finish the current step.
    /// </summary>
    public void Exit()
        => Exiting = true;

    /// <summary>
    ///     Called when the Game begins, after the Graphics and Platform initialization.
    /// </summary>
    public virtual void Begin()
    {
        foreach (var manager in Managers)
            manager.Begin();

        OnBegin?.Invoke();
    }

    /// <summary>
    ///     Called when the Game ends, before the Graphics and Platform destructor.
    /// </summary>
    public virtual void End()
    {
        foreach (var manager in Managers)
            manager.End();

        OnEnd?.Invoke();
    }

    /// <summary>
    ///     Performs the Update events of the game and managers.
    /// </summary>
    public virtual void Update()
    {
        foreach (var manager in Managers)
        {
            if (manager.Active)
                manager.Update();
        }

        OnUpdate?.Invoke();
    }
    
    /// <summary>
    ///     Performs the Render events of the game and managers.
    /// </summary>
    public virtual void Render()
    {
        foreach (var manager in Managers)
        {
            if (manager.Visible)
                manager.RenderBegin();
        }

        foreach (var manager in Managers)
        {
            if (manager.Visible)
                manager.Render();
        }

        foreach (var manager in Managers)
        {
            if (manager.Visible)
                manager.RenderEnd();
        }

        OnRender?.Invoke();
    }

    /// <summary>
    ///     Add a <see cref="GameManager"/> to the game.
    /// </summary>
    /// <param name="manager">The manager to add.</param>
    public T Add<T>(T manager) where T : GameManager
    {
        Managers.Add(manager);
        Managers.Sort();

        if (Running)
            manager.Begin();
            
        return manager;
    }

    /// <summary>
    ///     Add a <see cref="GameManager"/> to the game.
    /// </summary>
    public T Add<T>() where T : GameManager, new()
        => Add(new T());

    /// <summary>
    ///     Gets the first <see cref="GameManager"/> of the given type.
    /// </summary>
    public T? Get<T>() where T : GameManager
        => Managers.Get<T>();

    /// <summary>
    ///     Remove a <see cref="GameManager"/> from the game.
    /// </summary>
    /// <param name="manager">The manager to remove.</param>
    public T Remove<T>(T manager) where T : GameManager
    {
        Managers.Remove(manager);

        if (Running)
            manager.End();
            
        return manager;
    }
}