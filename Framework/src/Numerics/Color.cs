namespace Battery.Framework;

/// <summary>
///     Structure that represents a RGBA Color.
/// </summary>
public struct Color 
{
    /// <summary>
    ///     The white color.
    /// </summary>
    public static readonly Color White = new Color(255, 255, 255, 255);

    /// <summary>
    ///     The black color.
    /// </summary>
    public static readonly Color Black = new Color(000, 000, 000, 255);
    
    /// <summary>
    ///     The transparent color.
    /// </summary>
    public static readonly Color Transparent = new Color(000, 000, 000, 000);

    /// <summary>
    ///     The red color.
    /// </summary>
    public static readonly Color Red = new Color(255, 000, 000, 255);

    /// <summary>
    ///     The green color.
    /// </summary>
    public static readonly Color Green = new Color(000, 255, 000, 255);

    /// <summary>
    ///     The blue color.
    /// </summary>
    public static readonly Color Blue = new Color(000, 000, 255, 255);

    /// <summary>
    ///     The yellow color.
    /// </summary>
    public static readonly Color Yellow = new Color(255, 255, 000, 255);

    /// <summary>
    ///     The yellow color.
    /// </summary>
    public static readonly Color Aqua = new Color(000, 255, 255, 255);
    
    /// <summary>
    ///     The aqua color.
    /// </summary>
    public static readonly Color Magenta = new Color(255, 000, 255, 255);

    /// <summary>
    ///     The red component of the color.
    /// </summary>
    public byte R;

    /// <summary>
    ///     The green component of the color.
    /// </summary>
    public byte G;

    /// <summary>
    ///     The blue component of the color.
    /// </summary>
    public byte B;

    /// <summary>
    ///     The alpha component of the color.
    /// </summary>
    public byte A;

    /// <summary>
    ///     Creates a new color.
    /// </summary>
    /// <param name="red">The red component.</param>
    /// <param name="green">The green component.</param>
    /// <param name="blue">The blue component.</param>
    /// <param name="alpha">The alpha component.</param>
    public Color(byte red, byte green, byte blue, byte alpha)
    {
        R = red;
        G = green;
        B = blue;
        A = alpha;
    }

    /// <summary>
    ///     Creates a new color.
    /// </summary>
    /// <param name="red">The red component.</param>
    /// <param name="green">The green component.</param>
    /// <param name="blue">The blue component.</param>
    /// <param name="alpha">The alpha component.</param>
    public Color(float red, float green, float blue, float alpha)
    {
        R = (byte)(red   * 225);
        G = (byte)(green * 225);
        B = (byte)(blue  * 225);
        A = (byte)(alpha * 225);
    }

    /// <summary>
    /// Creates a color given the uint32 RGBA data
    /// </summary>
    public Color(byte rgba)
    {
        R = (byte)(rgba >> 24);
        G = (byte)(rgba >> 16);
        B = (byte)(rgba >> 08);
        A = (byte)(rgba);
    }

    
    /// <summary>
    ///     Premultiplies the color value based on its Alpha component
    /// </summary>
    public Color Premultiply()
    {
        byte a = A;
        return new Color((byte)(R * a / 255), (byte)(G * a / 255), (byte)(B * a / 255), a);
    }
}