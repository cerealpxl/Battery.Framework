using System.Numerics;
using SDL2;

namespace Battery.Framework;

/// <summary>
///     A SDL2 window platform.
/// /// </summary>
public class SDLPlatform : GamePlatform
{

    /// <inheritdoc />
    public override string Title 
    { 
        get
        {
            if (!_opened)
                throw new Exception("The window is not started.");
            
            return SDL.SDL_GetWindowTitle(_window);
        }
        set
        {
            if (!_opened)
                throw new Exception("The window is not started.");

            SDL.SDL_SetWindowTitle(_window, value);
        }
    }

    /// <inheritdoc />
    public override Vector2 Dimensions
    {
        get
        {
            if (!_opened)
                throw new Exception("The window is not started.");

            SDL.SDL_GetWindowSize(_window, out var width, out var height);
            return new Vector2(width, height);
        }
        set
        {
            if (!_opened)
                throw new Exception("The window is not started.");

            SDL.SDL_SetWindowSize(_window, (int)value.X, (int)value.Y);
        }
    }

    /// <inheritdoc />
    public override bool Fullscreen 
    { 
        get => _fullscreen;
        set => _fullscreen = value;
    }

    /// <inheritdoc />
    public override bool Borderless 
    { 
        get => _borderless;
        set => _borderless = value;
    }

    /// <inheritdoc />
    public override bool VSync 
    { 
        get => _vsync;
        set => _vsync = value;
    }

    /// <inheritdoc />
    public override bool Visible 
    { 
        get
        {
            if (!_opened)
                throw new Exception("The window is not started.");
            
            return _visible;
        }
        set
        {
            if (!_opened)
                throw new Exception("The window is not started.");

            if (value)
                SDL.SDL_ShowWindow(_window);
            else
                SDL.SDL_HideWindow(_window);

            _visible = value;
        }
    }

    /// <inheritdoc />
    public override bool Focused 
    { 
        get
        {
            if (!_opened)
                throw new Exception("The window is not started.");
            
            return _focused;
        }
    }

    // The window pointer.
    internal IntPtr _window;

    // Whether the window has been oepened.
    private bool _opened;

    // Whether the window is in the fullscreen mode.
    private bool _fullscreen;

    // Whether the window has no border.
    private bool _borderless;

    // Whether the vertical synchronization is enabled.
    private bool _vsync;

    // Whether the window is focused.
    private bool _focused;

    // Whether the window is visible.
    private bool _visible;

    /// <summary>
    ///     Initializes a new instance of <see cref="SDLPlatform" /> class.
    /// </summary>
    /// <param name="instance">The game to which the platform belongs to.</param>
    public SDLPlatform(Game instance)
        : base(instance)
    {
        if (SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING) != 0)
            throw new Exception(SDL.SDL_GetError());
    }

    /// Finalize from the SDL.
    ~SDLPlatform()
    {
        SDL.SDL_Quit();
    }

    /// <summary>
    ///     Begins the SDL2 Window.
    /// </summary>
    /// <param name="title">The title of the window.</param>
    /// <param name="width">The width of the window.</param>
    /// <param name="height">The height of the window.</param>
    /// <param name="fullscreen">Whether the window starts in the fullscreen mode.</param>
    /// <param name="borderless">Whether the window starts without a border.</param>
    public override void Begin(string title, int width, int height, bool fullscreen = false, bool borderless = false, bool vsync = false)
    {
        // Setup the OpenGL Attributes before creating the window.
        if (Game.Graphics is OpenGLGraphics)
        {
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_MAJOR_VERSION, 3);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_MINOR_VERSION, 3);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_MINOR_VERSION, 3);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_PROFILE_MASK, 1);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_FLAGS, 2);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_DOUBLEBUFFER, 1);
        }

        // Assign the window flags.
        var flags = SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN;

        if (Game.Graphics is OpenGLGraphics) flags |= SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL;
        if (fullscreen) flags |= SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP;
        if (borderless) flags |= SDL.SDL_WindowFlags.SDL_WINDOW_BORDERLESS;

        // Finally, creates the window.
        _window = SDL.SDL_CreateWindow(
            title,
            SDL.SDL_WINDOWPOS_CENTERED,
            SDL.SDL_WINDOWPOS_CENTERED,
            width,
            height,
            flags
        );

        // Error handling.
        if (_window == IntPtr.Zero)
            throw new Exception(SDL.SDL_GetError());

        // Assign varables.
        _opened = _visible = _focused = true;
        _fullscreen = fullscreen;
        _borderless = borderless;
        _vsync      = vsync;
    }

    /// <summary>
    ///     Destroys the SDL2 Window.
    /// </summary>
    public override void End()
        => SDL.SDL_DestroyWindow(_window);

    /// <summary>
    ///     Poll the SDL Window events.
    /// </summary>
    public override void Update()
    {
        while (SDL.SDL_PollEvent(out var ev) != 0)
        {
            switch (ev.type)
            {
                case SDL.SDL_EventType.SDL_WINDOWEVENT:
                    switch (ev.window.windowEvent)
                    {
                        case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE:
                            Game.Exit();
                            OnExit?.Invoke();
                        break;
                        case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_GAINED:
                            _focused = true;
                            OnFocusGained?.Invoke();
                        break;
                        case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_LOST:
                            _focused = false;
                            OnFocusLost?.Invoke();
                        break;
                    }
                break;
                case SDL.SDL_EventType.SDL_QUIT:
                    OnExit?.Invoke();
                    Game.Exit();
                break;
            }
        }
    }

    /// <summary>
    ///     Presents the Draw Data to the SDL Window.
    /// </summary>
    public override void Present()
    {
        OnRender?.Invoke();

        if (Game.Graphics is OpenGLGraphics glGraphics)
        {
            SDL.SDL_GL_MakeCurrent(_window, glGraphics._glPointer);
            SDL.SDL_GL_SetSwapInterval(_vsync == true ? 1 : 0);
            SDL.SDL_GL_SwapWindow(_window);
        }
    }
}