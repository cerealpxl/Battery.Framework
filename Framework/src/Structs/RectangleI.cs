using System.Numerics;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Battery.Framework;

/// <summary>
///     Structure that represents a 2D Integer Rectangle.
/// </summary>
public struct RectangleI
{
    /// <summary>
    ///     The horizontal position of the rectangle.
    /// </summary>
    public int X;

    /// <summary>
    ///     The vertical position of the rectangle.
    /// </summary>
    public int Y;

    /// <summary>
    ///     The width of the rectangle.
    /// </summary>
    public int Width;

    /// <summary>
    ///     The height of the rectangle.
    public int Height;

    /// <summary>
    ///     Left of the rectangle.
    /// </summary>
    [JsonIgnore]
    public int Left
    {
        get => X;
        set => X = value;
    }

    /// <summary>
    ///     Right of the rectangle.
    /// </summary>
    [JsonIgnore]
    public int Right
    {
        get => X + Width;
        set => X = value - Width;
    }

    /// <summary>
    ///     Top of the rectangle.
    /// </summary>
    [JsonIgnore]
    public int Top
    {
        get => Y;
        set => Y = value;
    }

    /// <summary>
    ///     Bottom of the rectangle.
    /// </summary>
    [JsonIgnore]
    public int Bottom 
    {
        get => Y + Height;
        set => Y = value - Height;
    }
    
    /// <summary>
    ///     The position of the rectangle.
    /// </summary>
    [JsonIgnore]
    public Vector2 Position
    { 
        get => new Vector2(X, Y);
        set
        {
            X = (int)value.X;
            Y = (int)value.Y;
        }
    }

    /// <summary>
    ///     The size of the rectangle.
    /// </summary>
    [JsonIgnore]
    public Vector2 Size
    { 
        get => new Vector2(Width, Height);
        set
        {
            Width  = (int)value.X;
            Height = (int)value.Y;
        }
    }

    /// <summary>
    ///     The area of the rectangle.
    /// </summary>
    [JsonIgnore]

    public int Area => Math.Abs(Width) * Math.Abs(Height);

    /// <summary>
    ///     The top left of the rectangle.
    /// </summary>
    [JsonIgnore]
    public Vector2 TopLeft
    {
        get => new Vector2(X, Y);
        set
        {
            X = (int)(value.X);
            Y = (int)(value.Y);
        }
    }

    /// <summary>
    ///     The bottom left of the rectangle.
    /// </summary>
    [JsonIgnore]
    public Vector2 BottomLeft
    {
        get => new Vector2(X, Y + Height);
        set
        {
            X = (int)(value.X);
            Y = (int)(value.Y - Height);
        }
    }

    /// <summary>
    ///     The top right of the rectangle.
    /// </summary>
    [JsonIgnore]
    public Vector2 TopRight
    {
        get => new Vector2(X + Width, Y);
        set
        {
            X = (int)(value.X - Width);
            Y = (int)(value.Y);
        }
    }

    /// <summary>
    ///     The bottom right of the rectangle.
    /// </summary>
    [JsonIgnore]

    public Vector2 BottomRight
    {
        get => new Vector2(X + Width, Y + Height);
        set
        {
            X = (int)(value.X - Width);
            Y = (int)(value.Y - Height);
        }
    }

    /// <summary>
    ///     The center of the rectangle.
    /// </summary>
    [JsonIgnore]

    public Vector2 Center
    {
        get => new Vector2(X + Width/2, Y + Height/2);
        set
        {
            X = (int)(value.X - Width/2);
            Y = (int)(value.Y - Height/2);
        }
    }


    /// <summary>
    ///     Creates a new float rectangle.
    /// </summary>
    /// <param name="x">The horizontal position of the rectangle.</param>
    /// <param name="y">The vertical position of the rectangle.</param>
    /// <param name="width">The width of the rectangle.</param>
    /// <param name="height">The height of the rectangle.</param>
    public RectangleI(int x, int y, int width, int height)
    {
        X      = x;
        Y      = y;
        Width  = width;
        Height = height;
    }

    /// <summary>
    ///     Returns a rectangle clamped inside that.
    /// </summary>
    /// <param name="against">The rectangle to check.</param>
    public RectangleI OverlapRect(in RectangleI against)
    {
        var overlapX = X + Width > against.X && X < against.X + against.Width;
        var overlapY = Y + Height > against.Y && Y < against.Y + against.Height;

        RectangleI r = new RectangleI();

        if (overlapX)
        {
            r.Left = Math.Max(Left, against.Left);
            r.Width = Math.Min(Right, against.Right) - r.Left;
        }

        if (overlapY)
        {
            r.Top = Math.Max(Top, against.Top);
            r.Height = Math.Min(Bottom, against.Bottom) - r.Top;
        }

        return r;
    }

    /// <summary>
    ///     Check collision between two rectangles.
    /// </summary>
    /// <param name="value">Rectangle to check.</param>
    public bool Intersects(RectangleI value)
        => value.X < Right && X < value.Right && value.Y < Bottom && Y < value.Bottom;

    /// <summary>
    ///     Transforms this rectangle to a readable string.
    /// </summary>
    public override string ToString()
        => $"<{X}, {Y}, {Width}, {Height}>";

    /// <summary>
    ///     Transforms a integer rectangle to a float rectangle.
    /// </summary>
    /// <param name="rectangle">Rectangle to transform.</param>
    public static implicit operator Rectangle(RectangleI rectangle)
        => new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);

    /// <summary>
    ///     Add to Vector2 operator.
    /// </summary>
    public static RectangleI operator +(RectangleI rectangle, Vector2 vec)
        => new RectangleI(rectangle.X + (int)vec.X, rectangle.Y + (int)vec.Y, rectangle.Width, rectangle.Height);

    /// <summary>
    ///     Subtract to Vector2 operator.
    /// </summary>
    public static RectangleI operator -(RectangleI rectangle, Vector2 vec)
        => new RectangleI(rectangle.X - (int)vec.X, rectangle.Y - (int)vec.Y, rectangle.Width, rectangle.Height);

    /// <summary>
    ///     Multiply to Vector2 operator.
    /// </summary>
    public static RectangleI operator *(RectangleI rectangle, Vector2 vec)
        => new RectangleI(rectangle.X * (int)vec.X, rectangle.Y * (int)vec.Y, rectangle.Width, rectangle.Height);

    /// <summary>
    ///     Divide to Vector2 operator.
    /// </summary>
    public static RectangleI operator /(RectangleI rectangle, Vector2 vec)
        => new RectangleI(rectangle.X / (int)vec.X, rectangle.Y / (int)vec.Y, rectangle.Width, rectangle.Height);
}