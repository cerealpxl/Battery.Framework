namespace Battery;

/// <summary>
///     Calcule functions.
/// </summary>
public static class Calc
{
    /// <summary>
    ///     Approach a value to the target value using the given amount.
    /// </summary>
    /// <param name="start">The start value.</param>
    /// <param name="target">The target value.</param>
    /// <param name="amount">The amount to use.</param>
    public static float Approach(float start, float target, float amount)
        => (start > target) ? Math.Max(start - amount, target) : Math.Min(start + amount, target);

    /// <summary>
    ///     Returns whether this number is negative, positive or zero.
    /// </summary>
    public static float Sign(this float num)
        => Math.Sign(num);
        
    /// <summary>
    ///     Returns whether this number is negative, positive or zero.
    /// </summary>
    public static int Sign(this int num)
        => Math.Sign(num);

    /// <summary>
    ///     Returns the absolute value of the float.
    /// </summary>
    public static float Abs(this float num)
        => Math.Abs(num);
        
    /// <summary>
    ///     Returns the absolute value of the integer.
    /// </summary>
    public static int Abs(this int num)
        => Math.Abs(num);
}
