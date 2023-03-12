using OpenGL;

namespace Battery.Framework;

/// <summary>
///     A single OpenGL Surface/Framebuffer.
/// </summary>
public class OpenGLSurface : Surface
{
    /// <summary>
    ///     Framebuffer ID.
    /// </summary>
    internal uint _framebufferID { get; private set; }

    /// <summary>
    ///     Creates a new instance of <see cref="OpenGLSurface"/> class.
    /// </summary>
    /// <param name="width">The Width of the Surface.</param>
    /// <param name="height">The Height of the Surface.</param>
    public OpenGLSurface(GameGraphics graphics, int width, int height) 
        : base(graphics, width, height)
    {
        if (Attachment is OpenGLTexture attachment)
        {
            _framebufferID = GL.glGenFramebuffer();

            GL.glBindFramebuffer(_framebufferID);
            GL.glFramebufferTexture2D(GL.GL_FRAMEBUFFER, GL.GL_COLOR_ATTACHMENT0, GL.GL_TEXTURE_2D, attachment.ID, 0);
            GL.glDrawBuffer(GL.GL_COLOR_ATTACHMENT0);
            GL.glBindFramebuffer(0u);

            Attachment.FlipY = true;
        }
    }

    /// <summary>
    ///     Dispose the OpenGL Surface by deleting its framebuffer and disposing the Texture Attachment.
    /// </summary>
    public override void Dispose()
    {
        GL.glDeleteFramebuffer(_framebufferID);
        Attachment.Dispose();
    }
}