namespace Battery.Framework;

/// <summary>
///     A surface that can be drawn to.
/// </summary>
public abstract class Surface : RenderTarget, IDisposable
{
    /// <summary>
    ///     The <see cref="Texture" /> attached to the Surface.
    /// </summary>
    public Texture Attachment { get; private set; }

    /// <summary>
    ///     The Width of the Surface, in Pixels.
    /// </summary>
    public override int Width => Attachment.Width;

    /// <summary>
    ///     The Height of the Surface, in Pixels.
    /// </summary>
    public override int Height => Attachment.Height;

    /// <summary>
    ///     Creates a new instance of the <see cref="Surface" /> class.
    /// </summary>
    /// <param name="graphics">The Actual Graphics backend.</param>
    /// <param name="width">The Width of the Surface.</param>
    /// <param name="height">The Height of the Surface.</param>
    public Surface(GameGraphics graphics, int width, int height) 
        : base()
    {
        // Creates the texture attachment.
        Attachment = graphics.CreateTexture(new Image(width, height, Color.Transparent));
    }

    /// <summary>
    ///     Dispose this Surface.
    /// </summary>
    public abstract void Dispose();

    /// <summary>
    ///     Returns the Texture attachment of the surface.
    /// </summary>
    /// <param name="surface">The surface to use.</param>
    public static implicit operator Texture(Surface surface)
        => surface.Attachment;
}