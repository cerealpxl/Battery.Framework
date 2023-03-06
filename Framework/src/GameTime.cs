using System.Diagnostics;

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

    // Stack that store some time values.
    private Stack<double> _stack = new Stack<double>();

    // Stopwatch used for timing operations.
    private Stopwatch _stopwatch = Stopwatch.StartNew();

    /// <summary>
    ///     Creates a new instance of the <see cref="GameTime" /> struct.
    /// </summary>
    public GameTime()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    internal void Update()
    {
        PreviousElapsed = Elapsed;
        Elapsed         = _stopwatch.Elapsed;
        RawDelta        = (float)(Elapsed - PreviousElapsed).TotalSeconds;
    }

    /// <summary>
    ///     Pushes the current time to measure the duration of an event.
    /// </summary>
    /// <param name="text">The text to write in the console.</param>
    public void Push(string text)
    {
        Console.WriteLine(text);
        Pop();
    }

    /// <summary>
    ///     Pushes the current time to measure the duration of an event.
    /// </summary>
    public void Push()
        => _stack.Push(_stopwatch.ElapsedMilliseconds);
    
    /// <summary>
    ///     Pop the stored time and calcule the time since it.
    /// </summary>
    /// <param name="text">The text to write in the console.</param>
    public void Pop(string text)
        => Console.WriteLine(text + $" [elapsed: {_stopwatch.ElapsedMilliseconds - Pop()}ms]");

    /// <summary>
    ///     Pop the stacked time and return the calcule of the time since it.
    /// </summary>
    public double Pop()
        => _stopwatch.ElapsedMilliseconds - _stack.Pop();
}