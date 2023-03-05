namespace Battery.Framework;

/// <summary>
///     A surface that can be drawn to.
/// </summary>
public abstract class Surface : RenderTarget, IDisposable
{
    /// <summary>
    ///     The <see cref="Texture" /> attached to the canvas.
    /// </summary>
    public Texture Attachment { get; private set; }

    /// <summary>
    ///     The width of the <see cref="Surface" />.
    /// </summary>
    public override int Width => Attachment.Width;

    /// <summary>
    ///     The height of the <see cref="Surface" />.
    /// </summary>
    public override int Height => Attachment.Height;

    /// <summary>
    ///     Creates a new instance of the <see cref="Surface" /> class.
    /// </summary>
    /// <param name="graphics">The actual graphics backend.</param>
    /// <param name="width">The width of the Surface.</param>
    /// <param name="height">The height of the Surface.</param>
    public Surface(GameGraphics graphics, int width, int height) 
        : base()
    {
        // Creates the texture attachment.
        Attachment = graphics.CreateTexture(new Image(width, height, Color.Transparent));
    }

    /// <summary>
    ///     Dispose this surface.
    /// </summary>
    public abstract void Dispose();

    /// <summary>
    ///     Transforms a integer rectangle to a float rectangle.
    /// </summary>
    /// <param name="rectangle">Rectangle to transform.</param>
    public static implicit operator Texture(Surface surface)
        => surface.Attachment;
}