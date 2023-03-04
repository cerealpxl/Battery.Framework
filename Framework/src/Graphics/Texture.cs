using System.Buffers;

namespace Battery.Framework;

/// <summary>
///     Represents a Graphics Texture, used to render images.
/// </summary>
public abstract class Texture : Graphic
{
    /// <summary>
    ///     The image that store the texture data.
    /// </summary>
    public Image Image;

    /// <summary>
    ///     The width of the texture.
    /// </summary>
    public int Width => Image.Width;

    /// <summary>
    ///     The height of the texture.
    /// </summary>
    public int Height => Image.Height;

    /// <summary>
    ///     Whether the image will be flipped horizontally when rendered.
    /// </summary>
    public bool FlipX;

    /// <summary>
    ///     Whether the image will be flipped vertically when rendered.
    /// </summary>
    public bool FlipY;

    /// <summary>
    ///     Creates a new texture with the given image.
    /// </summary>
    /// <param name="image">The image that represents the image.</param>
    public Texture(Image image)
        => Image = image;

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