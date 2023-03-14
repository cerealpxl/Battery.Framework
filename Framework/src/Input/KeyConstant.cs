namespace Battery.Framework;

/// <summary>
///     Represents the keyboard keys.
/// </summary>
public enum KeyConstant
{
    Unknown,

    #region Alphabet
    A,
    B,
    C,
    D,
    E,
    F,
    G,
    H,
    I,
    J,
    K,
    L,
    M,
    N,
    O,
    P,
    Q,
    R,
    S,
    T,
    U,
    V,
    W,
    X,
    Y,
    Z,
    #endregion
    
    Space,

    #region Digits keys
    D0,
    D1,
    D2,
    D3,
    D4,
    D5,
    D6,
    D7,
    D8,
    D9,
    #endregion
    
    #region Navigation keys
    Left,
    Right,
    Up,
    Down,
    Escape,
    Home,
    End,
    PageUp,
    PageDown,
    #endregion
    
    #region Editing keys
    Insert,
    Backspace,
    Tab,
    Clear,
    ///<summary>
    /// Also known as Enter.
    ///</summary>
    Return,
    Delete,
    #endregion
    
    #region Function keys
    F1,
    F2,
    F3,
    F4,
    F5,
    F6,
    F7,
    F8,
    F9,
    F10,
    F11,
    F12,
    #endregion
    
    #region Modifier keys
    CapsLock,
    ScrollLock,
    NumLock,
    LeftShift,
    RightShift,
    LeftCtrl,
    RightCtrl,
    LeftAlt,
    RightAlt,
    #endregion
}