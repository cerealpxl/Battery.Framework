namespace Battery.Framework;

/// <summary>
///     Structure that store all the time values.
/// </summary>
public struct GameTime
{
    /// <summary>
    ///     The current total elapsed time, in Seconds.
    /// </summary>
    public TimeSpan Elapsed = TimeSpan.Zero;

    /// <summary>
    ///     The elapsed time of the last frame, in Seconds.
    /// </summary>
    public TimeSpan PreviousElapsed = TimeSpan.Zero;

    /// <summary>
    ///     The time elapsed since the last frame, in Seconds.
    /// </summary>
    public float Delta => RawDelta * DeltaScale;

    /// <summary>
    ///     The real delta time.
    /// </summary>
    public float RawDelta = 0f;

    /// <summary>
    ///     The default delta time of the Fixed Update event.
    /// </summary>
    public float FixedDelta => 1 / FrameRate;

    /// <summary>
    ///     Value used to multiply the raw delta time.
    /// </summary>
    public float DeltaScale = 1f;

    /// <summary>
    ///     The number of frames per second.
    /// </summary>
    public float FrameRate = 60f;

    /// <summary>
    ///     Creates a new instance of the <see cref="GameTime" /> struct.
    /// </summary>
    public GameTime()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="elapsed"></param>
    /// <param name="previousElapsed"></param>
    internal void Update(TimeSpan elapsed, TimeSpan previousElapsed)
    {
        Elapsed         = elapsed;
        PreviousElapsed = previousElapsed;
        RawDelta        = (float)(Elapsed - PreviousElapsed).TotalSeconds;
    }
}