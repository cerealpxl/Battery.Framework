using System.Numerics;

namespace Battery.Framework;

public static class Mouse
{
    /// <summary>
    ///     The position of the mouse in the window.
    /// </summary>
    public static Vector2 Position { get; private set; } = Vector2.Zero;

    public static Vector2 Wheel { get; private set; } = Vector2.Zero;

    // A list storing pressing mouse buttons.
    private static HashSet<MouseButton> _down = new HashSet<MouseButton>();
    
    // A list storing pressed mouse buttons.
    private static HashSet<MouseButton> _pressed = new HashSet<MouseButton>();

    // A list storing released mouse buttons.
    private static HashSet<MouseButton> _released = new HashSet<MouseButton>();

    /// <summary>
    ///     Updates the mouse state.
    /// </summary>
    internal static void Update()
    {
        _pressed.Clear();
        _released.Clear();
    }

    /// <summary>
    ///     Checks if the given mouse button is held.
    /// </summary>
    /// <param name="button">The button to check.</param>
    public static bool Down(MouseButton button) 
        => _down.Contains(button);

    /// <summary>
    ///     Checks if any of the given mouse buttons were held.
    /// </summary>
    /// <param name="button1">The first button to check.</param>
    /// <param name="button2">The second button to check.</param>
    public static bool Down(MouseButton button1, MouseButton button2)
        => _down.Contains(button1) || _down.Contains(button2);

    /// <summary>
    ///     Checks if the given mouse button was released.
    /// </summary>
    /// <param name="button">The button to check.</param>
    public static bool Released(MouseButton button)
        => _released.Contains(button);

    /// <summary>
    ///     Checks if any of the given mouse buttons were released.
    /// </summary>
    /// <param name="button1">The first button to check.</param>
    /// <param name="button2">The second button to check.</param>
    public static bool Released(MouseButton button1, MouseButton button2)
        => _released.Contains(button1) || _released.Contains(button2);

    /// <summary>
    ///     Checks if the given mouse button was pressed.
    /// </summary>
    /// <param name="button">The button to check.</param>
    public static bool Pressed(MouseButton button)
        => _pressed.Contains(button);

    /// <summary>
    ///     Checks if any of the given buttons were pressed.
    /// </summary>
    /// <param name="button1">The first button to check.</param>
    /// <param name="button2">The second button to check.</param>
    public static bool Pressed(MouseButton button1, MouseButton button2)
        => _pressed.Contains(button1) || _pressed.Contains(button2);
    
    /// <summary>
    ///     Sets the style of the mouse cursor.
    /// </summary>
    /// <param name="cursor">The style to set.</param>
    public static void SetCursor(MouseCursor cursor)
    {
        if (Game.Instance != null)
            Game.Instance.Platform.SetMouseCursor(cursor);
    }
    
    /// <summary>
    ///     Gets the current style of the mouse cursor.
    /// </summary>
    public static MouseCursor GetCursor()
    {
        if (Game.Instance != null)
            return Game.Instance.Platform.GetMouseCursor();

        return MouseCursor.Arrow;
    }

    /// <summary>
    ///     Updates the state of the given button.
    /// </summary>
    /// <param name="button">The button to register.</param>
    public static void DoMouseDown(MouseButton button)
    {
        if (_down.Contains(button))
            return;
        
        // Update hash sets.
        _down.Add(button);
        _pressed.Add(button);
    }

    /// <summary>
    ///     Updates the state of the given button.
    /// </summary>
    /// <param name="button">The button to register.</param>
    public static void DoMouseUp(MouseButton button)
    {
        if (!_down.Contains(button))
            return;

        // Update hash sets.
        _down.Remove(button);
        _released.Add(button);
    }

    /// <summary>
    ///     Updates the position of the mouse cursor.
    /// </summary>
    /// <param name="x">The horizontal position of the cursor relative to the window.</param>
    /// <param name="y">The vertical position of the cursor relative to the window.</param>
    public static void DoMouseMotion(int x, int y) 
        => Position = new Vector2(x, y);

    /// <summary>
    ///     Updates the position of the mouse wheel.
    /// </summary>
    /// <param name="x">The horizontal offset of the wheel.</param>
    /// <param name="y">The vertical offset of the wheel.</param>
    public static void DoMouseWheel(float x, float y)
        => Wheel = new Vector2(x, y);
}