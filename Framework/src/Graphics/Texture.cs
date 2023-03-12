using System.Numerics;

namespace Battery.Framework;

/// <summary>
///     Represents a 2D Texture, used in image rendering.
/// </summary>
public abstract class Texture : IDisposable
{
    /// <summary>
    ///     The image that store the texture data.
    /// </summary>
    public Image Image;
    
    /// <summary>
    ///     Gets the Width and Height of the Texture.
    /// </summary>
    public Vector2 Dimensions => new(Width, Height);

    /// <summary>
    ///     Gets the Width of the Texture.
    /// </summary>
    public int Width => Image.Width;

    /// <summary>
    ///     Gets the Height of the Texture.
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
    ///     Creates a new instance of the <see cref="Texture"/> class.
    /// </summary>
    /// <param name="image">An <see cref="Framework.Image"/> object storing the Texture pixels.</param>
    public Texture(Image image)
    {
        Image = image;
    }

    /// <summary>
    ///     Dispose the texture.
    /// </summary>
    public abstract void Dispose();

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