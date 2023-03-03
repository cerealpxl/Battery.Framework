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

                // Keyboard events.
                case SDL.SDL_EventType.SDL_KEYDOWN:
                    Input.DoKeyDown(GetKey(SDL.SDL_GetKeyFromScancode(ev.key.keysym.scancode)));
                break;
                case SDL.SDL_EventType.SDL_KEYUP:
                    Input.DoKeyUp(GetKey(SDL.SDL_GetKeyFromScancode(ev.key.keysym.scancode)));
                break;

                // Mouse events.
                case SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN:
                    Input.DoMouseDown(ev.button.button switch {
                        (int)SDL.SDL_BUTTON_LEFT   => MouseButton.Left,
                        (int)SDL.SDL_BUTTON_MIDDLE => MouseButton.Middle,
                        (int)SDL.SDL_BUTTON_RIGHT  => MouseButton.Right,

                        _ => MouseButton.None,
                    });
                break;
                case SDL.SDL_EventType.SDL_MOUSEBUTTONUP:
                    Input.DoMouseUp(ev.button.button switch {
                        (int)SDL.SDL_BUTTON_LEFT   => MouseButton.Left,
                        (int)SDL.SDL_BUTTON_MIDDLE => MouseButton.Middle,
                        (int)SDL.SDL_BUTTON_RIGHT  => MouseButton.Right,

                        _ => MouseButton.None,
                    });
                break;
                case SDL.SDL_EventType.SDL_MOUSEMOTION:
                    Input.DoMouseMotion(ev.motion.x, ev.motion.y);
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

    /// <summary>
    /// Parse a SDL Keycode to a KeyConstant.
    /// </summary>
    /// <param name="key">SDL Keycode.</param>
    private static KeyConstant GetKey(SDL.SDL_Keycode key)
    {
        switch (key)
        {
            // Alphabet keys.
            case SDL.SDL_Keycode.SDLK_a: return KeyConstant.A;
            case SDL.SDL_Keycode.SDLK_b: return KeyConstant.B;
            case SDL.SDL_Keycode.SDLK_c: return KeyConstant.C;
            case SDL.SDL_Keycode.SDLK_d: return KeyConstant.D;
            case SDL.SDL_Keycode.SDLK_e: return KeyConstant.E;
            case SDL.SDL_Keycode.SDLK_f: return KeyConstant.F;
            case SDL.SDL_Keycode.SDLK_g: return KeyConstant.G;
            case SDL.SDL_Keycode.SDLK_h: return KeyConstant.H;
            case SDL.SDL_Keycode.SDLK_i: return KeyConstant.I;
            case SDL.SDL_Keycode.SDLK_j: return KeyConstant.J;
            case SDL.SDL_Keycode.SDLK_k: return KeyConstant.K;
            case SDL.SDL_Keycode.SDLK_l: return KeyConstant.L;
            case SDL.SDL_Keycode.SDLK_m: return KeyConstant.M;
            case SDL.SDL_Keycode.SDLK_n: return KeyConstant.N;
            case SDL.SDL_Keycode.SDLK_o: return KeyConstant.O;
            case SDL.SDL_Keycode.SDLK_p: return KeyConstant.P;
            case SDL.SDL_Keycode.SDLK_q: return KeyConstant.Q;
            case SDL.SDL_Keycode.SDLK_r: return KeyConstant.R;
            case SDL.SDL_Keycode.SDLK_s: return KeyConstant.S;
            case SDL.SDL_Keycode.SDLK_t: return KeyConstant.T;
            case SDL.SDL_Keycode.SDLK_u: return KeyConstant.U;
            case SDL.SDL_Keycode.SDLK_v: return KeyConstant.V;
            case SDL.SDL_Keycode.SDLK_w: return KeyConstant.W;
            case SDL.SDL_Keycode.SDLK_x: return KeyConstant.X;
            case SDL.SDL_Keycode.SDLK_y: return KeyConstant.Y;
            case SDL.SDL_Keycode.SDLK_z: return KeyConstant.Z;

            case SDL.SDL_Keycode.SDLK_SPACE:  return KeyConstant.Space;

            // Numerics.
            case SDL.SDL_Keycode.SDLK_0: return KeyConstant.D0;
            case SDL.SDL_Keycode.SDLK_1: return KeyConstant.D1;
            case SDL.SDL_Keycode.SDLK_2: return KeyConstant.D2;
            case SDL.SDL_Keycode.SDLK_3: return KeyConstant.D3;
            case SDL.SDL_Keycode.SDLK_4: return KeyConstant.D4;
            case SDL.SDL_Keycode.SDLK_5: return KeyConstant.D5;
            case SDL.SDL_Keycode.SDLK_6: return KeyConstant.D6;
            case SDL.SDL_Keycode.SDLK_7: return KeyConstant.D7;
            case SDL.SDL_Keycode.SDLK_8: return KeyConstant.D8;
            case SDL.SDL_Keycode.SDLK_9: return KeyConstant.D9;

            // Navigation keys.
            case SDL.SDL_Keycode.SDLK_LEFT:  return KeyConstant.Left;
            case SDL.SDL_Keycode.SDLK_RIGHT: return KeyConstant.Right;
            case SDL.SDL_Keycode.SDLK_UP:    return KeyConstant.Up;
            case SDL.SDL_Keycode.SDLK_DOWN:  return KeyConstant.Down;
            
            case SDL.SDL_Keycode.SDLK_HOME:     return KeyConstant.Home;
            case SDL.SDL_Keycode.SDLK_END:      return KeyConstant.End;
            case SDL.SDL_Keycode.SDLK_PAGEUP:   return KeyConstant.PageUp;
            case SDL.SDL_Keycode.SDLK_PAGEDOWN: return KeyConstant.PageDown;

            // Editing keys.
            case SDL.SDL_Keycode.SDLK_INSERT:    return KeyConstant.Insert;
            case SDL.SDL_Keycode.SDLK_BACKSPACE: return KeyConstant.Backspace;
            case SDL.SDL_Keycode.SDLK_TAB:       return KeyConstant.Tab;
            case SDL.SDL_Keycode.SDLK_CLEAR:     return KeyConstant.Clear;
            case SDL.SDL_Keycode.SDLK_RETURN:    return KeyConstant.Return;
            case SDL.SDL_Keycode.SDLK_DELETE:    return KeyConstant.Delete;

            // Function keys.
            case SDL.SDL_Keycode.SDLK_F1:  return KeyConstant.F1;
            case SDL.SDL_Keycode.SDLK_F2:  return KeyConstant.F2;
            case SDL.SDL_Keycode.SDLK_F3:  return KeyConstant.F3;
            case SDL.SDL_Keycode.SDLK_F4:  return KeyConstant.F4;
            case SDL.SDL_Keycode.SDLK_F5:  return KeyConstant.F5;
            case SDL.SDL_Keycode.SDLK_F6:  return KeyConstant.F6;
            case SDL.SDL_Keycode.SDLK_F7:  return KeyConstant.F7;
            case SDL.SDL_Keycode.SDLK_F8:  return KeyConstant.F8;
            case SDL.SDL_Keycode.SDLK_F9:  return KeyConstant.F9;
            case SDL.SDL_Keycode.SDLK_F10: return KeyConstant.F10;
            case SDL.SDL_Keycode.SDLK_F11: return KeyConstant.F11;
            case SDL.SDL_Keycode.SDLK_F12: return KeyConstant.F12;

            // Modifier keys.
            case SDL.SDL_Keycode.SDLK_NUMLOCKCLEAR: return KeyConstant.NumLock;
            case SDL.SDL_Keycode.SDLK_SCROLLLOCK:   return KeyConstant.ScrollLock;
            case SDL.SDL_Keycode.SDLK_CAPSLOCK:     return KeyConstant.CapsLock;

            case SDL.SDL_Keycode.SDLK_LSHIFT: return KeyConstant.LeftShift;
            case SDL.SDL_Keycode.SDLK_RSHIFT: return KeyConstant.RightShift;
            case SDL.SDL_Keycode.SDLK_LCTRL:  return KeyConstant.LeftCtrl;
            case SDL.SDL_Keycode.SDLK_RCTRL:  return KeyConstant.RightCtrl;
            case SDL.SDL_Keycode.SDLK_LALT:   return KeyConstant.LeftAlt;
            case SDL.SDL_Keycode.SDLK_RALT:   return KeyConstant.RightAlt;
        }

        return KeyConstant.Unknown;
    }
}