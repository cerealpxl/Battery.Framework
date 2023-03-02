using System.Buffers;

namespace Battery.Framework;

/// <summary>
///     Represents a Graphics Texture, used to render images.
/// </summary>
public abstract class Texture : Graphic
{
    /// <summary>
    ///     The bitmap that store the texture data.
    /// </summary>
    public Bitmap Bitmap;

    /// <summary>
    ///     The width of the texture.
    /// </summary>
    public int Width => Bitmap.Width;

    /// <summary>
    ///     The height of the texture.
    /// </summary>
    public int Height => Bitmap.Height;

    /// <summary>
    ///     Whether the image will be flipped horizontally when rendered.
    /// </summary>
    public bool FlipX;

    /// <summary>
    ///     Whether the image will be flipped vertically when rendered.
    /// </summary>
    public bool FlipY;

    /// <summary>
    ///     Creates a new texture with the given bitmap.
    /// </summary>
    /// <param name="bitmap">The bitmap that represents the image.</param>
    /// <param name="flipX">Whether the image will be flipped horizontally.</param>
    /// <param name="flipY">Whether the image will be flipped vertically.</param>
    public Texture(Bitmap bitmap, bool flipX = false, bool flipY = false)
    {
        Bitmap = bitmap;
        FlipX  = flipX;
        FlipY  = flipY;
    }

    /// <summary>
    ///     Creates a new texture, loading a image in the specified path.
    /// </summary>
    /// <param name="path">Path to the image.</param>
    /// <param name="flipX">Whether the image will be flipped horizontally.</param>
    /// <param name="flipY">Whether the image will be flipped vertically.</param>
    public Texture(string path, bool flipX = false, bool flipY = false)
        : this(new Bitmap(path), flipX, flipY)
    {
    }

    /// <summary>
    ///     Sets the texture data from the given buffer.
    /// </summary>
    /// <param name="buffer">Buffer to use.</param>
    public abstract void SetData<T>(ReadOnlyMemory<T> buffer);

    /// <summary>
    ///     Writes the texture to the given buffer.
    /// </summary>
    /// <param name="buffer">Buffer to write.</param>
    public abstract void GetData<T>(Memory<T> buffer);
}