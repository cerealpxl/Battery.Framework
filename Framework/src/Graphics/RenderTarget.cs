namespace Battery.Framework;

/// <summary>
///     An object that can be drawn to.
/// </summary>
public abstract class RenderTarget
{
    /// <summary>
    ///     The Width of the Target, in Pixels.
    /// </summary>
    public abstract int Width { get; }

    /// <summary>
    ///     The Height of the target, in Pixels.
    /// </summary>
    public abstract int Height { get; }
}