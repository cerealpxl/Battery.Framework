namespace Battery.Framework;

/// <summary>
///     An object that can be drawn to.
/// </summary>
public abstract class RenderTarget
{
    /// <summary>
    ///     The width of the target.
    /// </summary>
    public abstract int Width { get; }

    /// <summary>
    ///     The height of the target.
    /// </summary>
    public abstract int Height { get; }
}