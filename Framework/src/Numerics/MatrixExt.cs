using System.Numerics;

namespace Battery.Framework;

/// <summary>
///     Matrix method extensions.
/// </summary>
public static class MatrixExt
{
    /// <summary>
    ///     Transforms a vector by multiplying this matrix.
    /// </summary>
    /// <param name="matrix">Matrix3x2.</param>
    /// <param name="vec">The vector to transform.</param>
	public static Vector2 MultiplyVector(this Matrix3x2 matrix, in Vector2 vec)
	{
		return new Vector2(
            (vec.X * matrix.M11) + (vec.Y * matrix.M12) + matrix.M31,
            (vec.X * matrix.M21) + (vec.Y * matrix.M22) + matrix.M32
        );
	}

    /// <summary>
    ///     Creates a Matrix3x2 the specified transformations.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="origin"></param>
    /// <param name="scale"></param>
    /// <param name="rotation"></param>
    public static Matrix3x2 Transform(this Matrix3x2 matrix, in Vector2 position, in Vector2 origin, in Vector2 scale, in float rotation)
    {
        if (origin != Vector2.Zero)
            matrix *= Matrix3x2.CreateTranslation(-origin.X, -origin.Y);

        if (scale != Vector2.One)
            matrix *= Matrix3x2.CreateScale(scale.X, scale.Y);

        if (rotation != 0)
            matrix *= Matrix3x2.CreateRotation(rotation);

        if (position != Vector2.Zero)
            matrix *= Matrix3x2.CreateTranslation(position.X, position.Y);

        return matrix;
    }
}