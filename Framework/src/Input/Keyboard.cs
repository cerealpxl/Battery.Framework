namespace Battery.Framework;

public static class Keyboard
{
    /// <summary>
    ///     Whether the Left or Right Ctrl keys are held.
    /// </summary>
    public static bool Ctrl => Down(KeyConstant.LeftCtrl, KeyConstant.RightCtrl);

    /// <summary>
    ///     Whether the Left or Right Alt keys are held.
    /// </summary>
    public static bool Alt => Down(KeyConstant.LeftAlt, KeyConstant.RightAlt);

    /// <summary>
    ///     Whether the Left or Right Shift keys are held.
    /// </summary>
    public static bool Shift => Down(KeyConstant.LeftShift, KeyConstant.RightShift);

    /// <summary>
    ///     Called when a keyboard key is immediately down.
    /// </summary>
    public static Action<KeyConstant>? OnKeyDown;

    /// <summary>
    ///     Called when a keyboard key is immediately released.
    /// </summary>
    public static Action<KeyConstant>? OnKeyUp;

    // The list that store the pressing keys.
    private static HashSet<KeyConstant> _down = new HashSet<KeyConstant>();
    
    // The list that store the pressed keys.
    private static HashSet<KeyConstant> _pressed = new HashSet<KeyConstant>();
    
    // The list that store the released buttons.
    private static HashSet<KeyConstant> _released = new HashSet<KeyConstant>();
    
    /// <summary>
    ///     Updates the keyboard state.
    /// </summary>
    internal static void Update()
    {
        _pressed.Clear();
        _released.Clear();
    }

    /// <summary>
    ///     Check if the given key is down.
    /// </summary>
    /// <param name="key">The key to check.</param>
    public static bool Down(KeyConstant key)
        => _down.Contains(key);

    /// <summary>
    ///     Checks if any of the given keys were held.
    /// </summary>
    /// <param name="key1">The first key to check.</param>
    /// <param name="key2">The seconds key to check.</param>
    public static bool Down(KeyConstant key1, KeyConstant key2)
        => _down.Contains(key1) || _down.Contains(key2);

    /// <summary>
    ///     Checks if the given key was released.
    /// </summary>
    /// <param name="key">The key to check.</param>
    public static bool Released(KeyConstant key)
        => _released.Contains(key);

    /// <summary>
    ///     Checks if any of the given keys were released.
    /// </summary>
    /// <param name="key1">The first key to check.</param>
    /// <param name="key2">The seconds key to check.</param>
    public static bool Released(KeyConstant key1, KeyConstant key2)
        => _released.Contains(key1) || _released.Contains(key2);

    /// <summary>
    ///     Checks if the given key was pressed.
    /// </summary>
    /// <param name="key">The key to check.</param>
    public static bool Pressed(KeyConstant key)
        => _pressed.Contains(key);

    /// <summary>
    /// Checks if any of the given keys were pressed.
    /// </summary>
    /// <param name="key1">The first key to check.</param>
    /// <param name="key2">The seconds key to check.</param>
    public static bool Pressed(KeyConstant key1, KeyConstant key2)
        => _pressed.Contains(key1) || _pressed.Contains(key2);

    /// <summary>
    ///     Updates the state of the given key.
    /// </summary>
    /// <param name="key">The key to register.</param>
    public static void DoKeyDown(KeyConstant key)
    {
        OnKeyDown?.Invoke(key);

        if (_down.Contains(key))
            return;

        // Update hash sets.
        _down.Add(key);
        _pressed.Add(key);
    }

    /// <summary>
    ///     Updates the state of the given key.
    /// </summary>
    /// <param name="key">The key to register.</param>
    public static void DoKeyUp(KeyConstant key)
    {
        OnKeyUp?.Invoke(key);

        if (!_down.Contains(key))
            return;

        // Update hash sets.
        _down.Remove(key);
        _released.Add(key);
    }
}