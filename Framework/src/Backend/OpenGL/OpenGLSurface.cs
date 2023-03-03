using OpenGL;

namespace Battery.Framework;

/// <summary>
/// A surface that can be drawn to.
/// </summary>
public class OpenGLSurface : Surface
{
    /// <summary>
    ///     Framebuffer ID.
    /// </summary>
    public uint FramebufferID { get; private set; }

    /// <summary>
    ///     Creates a new canvas/framebuffer.
    /// </summary>
    /// <param name="width">The width of the canvas.</param>
    /// <param name="height">The height of the canvas.</param>
    /// <param name="filter">Filter of the canvas, GL_NEAREST by default.</param>
    public OpenGLSurface(GameGraphics graphics, int width, int height) 
        : base(graphics, width, height)
    {
        if (Attachment is OpenGLTexture attachment)
        {
            FramebufferID = GL.glGenFramebuffer();

            GL.glBindFramebuffer(FramebufferID);
            GL.glFramebufferTexture2D(GL.GL_FRAMEBUFFER, GL.GL_COLOR_ATTACHMENT0, GL.GL_TEXTURE_2D, attachment.ID, 0);
            GL.glDrawBuffer(GL.GL_COLOR_ATTACHMENT0);
            GL.glBindFramebuffer(0u);

            Attachment.FlipY = true;
        }
    }

    /// <summary>
    ///     Dispose the framebuffer.
    /// </summary>
    protected override void Disposing(bool disposing)
    {
        GL.glDeleteFramebuffer(FramebufferID);
        Attachment.Dispose();
    }
}