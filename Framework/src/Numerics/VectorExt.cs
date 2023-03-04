using System;
using System.Numerics;

namespace Battery.Framework;

/// <summary>
///     Vector method extensions.
/// </summary>
public static class VectorExt
{
    /// <summary>
    ///     Turns the Vector2 to its left perpendicular.
    /// </summary>
    /// <param name="vec">Vector to use.</param>
    public static Vector2 TurnRight(this Vector2 vec) => new Vector2(-vec.Y, vec.X);

    /// <summary>
    ///     Turns the Vector2 to its left perpendicular.
    /// </summary>
    /// <param name="vec">Vector to use.</param>
    public static Vector2 TurnLeft(this Vector2 vec) => new Vector2(vec.Y, -vec.X);

    /// <summary>
    ///     Rotate this vector.
    /// </summary>
    /// <param name="angle">Angle.</param>
    public static Vector2 ToAngle(this Vector2 vec, float angle)
        => new Vector2(MathF.Cos(angle) * vec.X, MathF.Sin(angle) * vec.Y);
    
    /// <summary>
    ///     Floors each component of the vector.
    /// </summary>
    public static Vector2 Floor(this Vector2 vec)
        => new Vector2(MathF.Floor(vec.X), MathF.Floor(vec.Y));

    /// <summary>
    ///     Floors each component of the vector.
    /// </summary>
    public static Vector3 Floor(this Vector3 vec)
        => new Vector3(MathF.Floor(vec.X), MathF.Floor(vec.Y), MathF.Floor(vec.Z));

    /// <summary>
    ///     Floors each component of the vector.
    /// </summary>
    public static Vector4 Floor(this Vector4 vec)
        => new Vector4(MathF.Floor(vec.X), MathF.Floor(vec.Y), MathF.Floor(vec.Z), MathF.Floor(vec.W));

    /// <summary>
    ///     Rounds each component of the vector.
    /// </summary>
    public static Vector2 Round(this Vector2 vec)
        => new Vector2(MathF.Round(vec.X), MathF.Round(vec.Y));

    /// <summary>
    ///     Rounds each component of the vector.
    /// </summary>
    public static Vector3 Round(this Vector3 vec)
        => new Vector3(MathF.Round(vec.X), MathF.Round(vec.Y), MathF.Round(vec.Z));

    /// <summary>
    ///     Rounds each component of the vector.
    /// </summary>
    public static Vector4 Round(this Vector4 vec)
        => new Vector4(MathF.Round(vec.X), MathF.Round(vec.Y), MathF.Round(vec.Z), MathF.Round(vec.W));

    /// <summary>
    ///     Ceils each component of the vector.
    /// </summary>
    public static Vector2 Ceiling(this Vector2 vec)
        => new Vector2(MathF.Ceiling(vec.X), MathF.Ceiling(vec.Y));

    /// <summary>
    ///     Ceils each component of the vector.
    /// </summary>
    public static Vector3 Ceiling(this Vector3 vec)
        => new Vector3(MathF.Ceiling(vec.X), MathF.Ceiling(vec.Y), MathF.Ceiling(vec.Z));

    /// <summary>
    ///     Ceils each component of the vector.
    /// </summary>
    public static Vector4 Ceiling(this Vector4 vec)
        => new Vector4(MathF.Ceiling(vec.X), MathF.Ceiling(vec.Y), MathF.Ceiling(vec.Z), MathF.Ceiling(vec.W));

    /// <summary>
    ///     Sets each component of the vector to its absolute value.
    /// </summary>
    public static Vector2 Abs(this Vector2 vec)
        => new Vector2(MathF.Abs(vec.X), MathF.Abs(vec.Y));

    /// <summary>
    ///     Sets each component of the vector to its absolute value.
    /// </summary>
    public static Vector3 Abs(this Vector3 vec)
        => new Vector3(MathF.Abs(vec.X), MathF.Abs(vec.Y), MathF.Abs(vec.Z));

    /// <summary>
    ///     Sets each component of the vector to its absolute value.
    /// </summary>
    public static Vector4 Abs(this Vector4 vec)
        => new Vector4(MathF.Abs(vec.X), MathF.Abs(vec.Y), MathF.Abs(vec.Z), MathF.Abs(vec.W));

    /// <summary>
    ///     Approaches the vector to the target value by the given amount.
    /// </summary>
    public static Vector2 Approach(this Vector2 vec, Vector2 target, Vector2 amount)
    {
        return new Vector2(
            Calc.Approach(vec.X, target.X, amount.X),
            Calc.Approach(vec.Y, target.Y, amount.Y)
        );
    }

    /// <summary>
    ///     Approaches the vector to the target value by the given amount.
    /// </summary>
    public static Vector3 Approach(this Vector3 vec, Vector3 target, Vector3 amount)
    {
        return new Vector3(
            Calc.Approach(vec.X, target.X, amount.X),
            Calc.Approach(vec.Y, target.Y, amount.Y),
            Calc.Approach(vec.Z, target.Z, amount.Z)
        );
    }

    /// <summary>
    ///     Approaches the vector to the target value by the given amount.
    /// </summary>
    public static Vector4 Approach(this Vector4 vec, Vector4 target, Vector4 amount)
    {
        return new Vector4(
            Calc.Approach(vec.X, target.X, amount.X),
            Calc.Approach(vec.Y, target.Y, amount.Y),
            Calc.Approach(vec.Z, target.Z, amount.Z),
            Calc.Approach(vec.W, target.W, amount.W)
        );
    }

    /// <summary>
    ///     Normalizes a Vector2 safely (a zero-length Vector2 returns 0).
    /// </summary>
    public static Vector2 Normalized(this Vector2 vector)
    {
        if (vector.X == 0 && vector.Y == 0)
            return Vector2.Zero;
            
        return Vector2.Normalize(vector);
    }

    /// <summary>
    ///     Normalizes a Vector3 safely (a zero-length Vector3 returns 0).
    /// </summary>
    public static Vector3 Normalized(this Vector3 vector)
    {
        if (vector.X == 0 && vector.Y == 0 && vector.Z == 0)
            return Vector3.Zero;
            
        return Vector3.Normalize(vector);
    }

    /// <summary>
    ///     Normalizes a Vector4 safely (a zero-length Vector4 returns 0).
    /// </summary>
    public static Vector4 Normalized(this Vector4 vector)
    {
        if (vector.X == 0 && vector.Y == 0 && vector.Z == 0 && vector.W == 0)
            return Vector4.Zero;
            
        return Vector4.Normalize(vector);
    }
}