using System.Diagnostics;

namespace Battery.Framework;

/// <summary>
///     Class that manages the time values of the game.
/// </summary>
public static class Time
{
    /// <summary>
    ///     The current total elapsed time, in Seconds.
    /// </summary>
    public static TimeSpan Elapsed { get; internal set; } = TimeSpan.Zero;

    /// <summary>
    ///     The elapsed time of the last frame, in Seconds.
    /// </summary>
    public static TimeSpan PreviousElapsed { get; internal set; } = TimeSpan.Zero;

    /// <summary>
    ///     The time elapsed since the last frame, in Seconds.
    ///     This will return the variable delta when outside the Fixed Update event.
    /// </summary>
    public static float Delta => RawDelta * DeltaScale;

    /// <summary>
    ///     The time elapsed since the last frame, in Seconds, not scaled by the <see cref="DeltaScale"/>.
    ///     This will return the variable delta when outside the Fixed Update event.
    /// </summary>
    public static float RawDelta { get; internal set; } = 0f;

    /// <summary>
    ///     The default delta time of the Fixed Update event.
    /// </summary>
    public static float FixedDelta => 1 / FrameRate;

    /// <summary>
    ///     Value used to multiply the raw delta time.
    /// </summary>
    public static float DeltaScale = 1f;

    /// <summary>
    ///     The current number of frames running per second.
    /// </summary>
    public static int FPS { get; internal set; } = 0;

    /// <summary>
    ///     The number of frames per second.
    /// </summary>
    public static float FrameRate = 60f;

    /// <summary>
    ///     The <see cref="Stopwatch"/> used for timing operations.
    /// </summary>
    public readonly static Stopwatch Watch = Stopwatch.StartNew();

    // Stack that store some time values.
    private static Stack<double> _stack = new Stack<double>();

    /// <summary>
    ///     Updates the Current Time state.
    /// </summary>
    internal static void Update()
    {
        PreviousElapsed = Elapsed;
        Elapsed         = Watch.Elapsed;
        RawDelta        = (float)(Elapsed - PreviousElapsed).TotalSeconds;
    
        if (Math.Abs(RawDelta - 1f / 120f) < 0.0002f) RawDelta = 1f / 120f;
        if (Math.Abs(RawDelta - 1f / 60f)  < 0.0002f) RawDelta = 1f / 60f;
        if (Math.Abs(RawDelta - 1f / 30f)  < 0.0002f) RawDelta = 1f / 30f;
        if (Math.Abs(RawDelta - 1f / 15f)  < 0.0002f) RawDelta = 1f / 15f;
    }

    /// <summary>
    ///     Pushes the current time to measure the duration of an event.
    /// </summary>
    /// <param name="text">The text to write in the console.</param>
    public static void Push(string text)
    {
        Console.WriteLine(text);
        Pop();
    }

    /// <summary>
    ///     Pushes the current time to measure the duration of an event.
    /// </summary>
    public static void Push()
    {
        _stack.Push(Watch.ElapsedMilliseconds);
    }

    /// <summary>
    ///     Pop the stacked time and measure the time since the push.
    /// </summary>
    /// <param name="text">The text to write in the console.</param>
    public static void Pop(string text)
    {
        Console.WriteLine(text + $" [elapsed: {Watch.ElapsedMilliseconds - Pop()}ms]");
    }

    /// <summary>
    ///     Pop the stacked time and measure the time since the push.
    /// </summary>
    public static double Pop()
    {
        return Watch.ElapsedMilliseconds - _stack.Pop();
    }
}