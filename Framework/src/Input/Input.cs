using System.Numerics;

namespace Battery.Framework;

/// <summary>
///     Class that manages the Input.
/// </summary>
public static class Input
{
    /// <summary>
    /// Update the keyboard and mouse states.
    /// </summary>
    internal static void Update()
    {
        _keyPressed.Clear();
        _keyReleased.Clear();
        _mousePressed.Clear();
        _mouseReleased.Clear();
    }

    #region Keyboard fields

    /// <summary>
    /// List storing pressing keys.
    /// </summary>
    private static HashSet<KeyConstant> _keyDown = new HashSet<KeyConstant>();
    
    /// <summary>
    /// List storing pressed keys.
    /// </summary>
    private static HashSet<KeyConstant> _keyPressed = new HashSet<KeyConstant>();
    
    /// <summary>
    /// List storing released buttons.
    /// </summary>
    private static HashSet<KeyConstant> _keyReleased = new HashSet<KeyConstant>();
    
    #endregion
    
    #region Keyboard check

    /// <summary>
    /// Checks if the given key is held.
    /// </summary>
    public static bool KeyDown(KeyConstant key)
        => _keyDown.Contains(key);

    /// <summary>
    /// Checks if any of the given keys were held.
    /// </summary>
    public static bool KeyDown(KeyConstant key1, KeyConstant key2)
        => _keyDown.Contains(key1) || _keyDown.Contains(key2);

    /// <summary>
    /// Checks if the given key was released.
    /// </summary>
    public static bool KeyReleased(KeyConstant key)
        => _keyReleased.Contains(key);

    /// <summary>
    /// Checks if any of the given keys were released.
    /// </summary>
    public static bool KeyReleased(KeyConstant key1, KeyConstant key2)
        => _keyReleased.Contains(key1) || _keyReleased.Contains(key2);

    /// <summary>
    /// Checks if the given key was pressed.
    /// </summary>
    public static bool KeyPressed(KeyConstant key)
        => _keyPressed.Contains(key);

    /// <summary>
    /// Checks if any of the given keys were pressed.
    /// </summary>
    public static bool KeyPressed(KeyConstant key1, KeyConstant key2)
        => _keyPressed.Contains(key1) || _keyPressed.Contains(key2);

    #endregion
    
    #region Keyboard events

    /// <summary>
    /// Updates the state of the given key.
    /// </summary>
    public static void DoKeyDown(KeyConstant key)
    {
        if (_keyDown.Contains(key))
            return;
        
        // Update hash sets.
        _keyDown.Add(key);
        _keyPressed.Add(key);
    }

    /// <summary>
    /// Updates the state of the given key.
    /// </summary>
    public static void DoKeyUp(KeyConstant key)
    {
        if (!_keyDown.Contains(key))
            return;

        // Update hash sets.
        _keyDown.Remove(key);
        _keyReleased.Add(key);
    }

    #endregion

    
    #region Mouse fields

    /// <summary>
    ///     Mouse position in the window.
    /// </summary>
    public static Vector2 MousePosition { get; private set; } = Vector2.Zero;

    /// <summary>
    ///     A list storing pressing mouse buttons.
    /// </summary>
    private static HashSet<MouseButton> _mouseDown = new HashSet<MouseButton>();
    
    /// <summary>
    ///     A list storing pressed mouse buttons.
    /// </summary>
    private static HashSet<MouseButton> _mousePressed = new HashSet<MouseButton>();

    /// <summary>
    ///     A list storing released mouse buttons.
    /// </summary>
    private static HashSet<MouseButton> _mouseReleased = new HashSet<MouseButton>();

    #endregion 
    
    #region Mouse check

    /// <summary>
    ///     Checks if the given mouse button is held.
    /// </summary>
    public static bool MouseDown(MouseButton button) 
        => _mouseDown.Contains(button);

    /// <summary>
    ///     Checks if any of the given mouse buttons were held.
    /// </summary>
    public static bool MouseDown(MouseButton button1, MouseButton button2)
        => _mouseDown.Contains(button1) || _mouseDown.Contains(button2);

    /// <summary>
    /// Checks if the given mouse button was released.
    /// </summary>
    public static bool MouseReleased(MouseButton button)
        => _mouseReleased.Contains(button);

    /// <summary>
    ///     Checks if any of the given mouse buttons were released.
    /// </summary>
    public static bool MouseReleased(MouseButton button1, MouseButton button2)
        => _mouseReleased.Contains(button1) || _mouseReleased.Contains(button2);

    /// <summary>
    ///     Checks if the given mouse button was pressed.
    /// </summary>
    public static bool MousePressed(MouseButton button)
        => _mousePressed.Contains(button);

    /// <summary>
    ///     Checks if any of the given buttons were pressed.
    /// </summary>
    public static bool MousePressed(MouseButton button1, MouseButton button2)
        => _mousePressed.Contains(button1) || _mousePressed.Contains(button2);
    
    #endregion
    
    #region Mouse events

    /// <summary>
    ///     Updates the state of the given button.
    /// </summary>
    public static void DoMouseDown(MouseButton button)
    {
        if (_mouseDown.Contains(button))
            return;
        
        // Update hash sets.
        _mouseDown.Add(button);
        _mousePressed.Add(button);
    }

    /// <summary>
    ///     Updates the state of the given button.
    /// </summary>
    public static void DoMouseUp(MouseButton button)
    {
        if (!_mouseDown.Contains(button))
            return;

        // Update hash sets.
        _mouseDown.Remove(button);
        _mouseReleased.Add(button);
    }

    /// <summary>
    ///     Updates the mouse position.
    /// </summary>
    public static void DoMouseMotion(int x, int y) 
        => MousePosition = new Vector2(x, y);

    #endregion
}