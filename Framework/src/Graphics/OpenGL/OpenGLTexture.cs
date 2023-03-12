using System.Buffers;
using OpenGL;

namespace Battery.Framework;

/// <summary>
///     A OpenGL texture.
/// </summary>
public class OpenGLTexture : Texture
{
    /// <summary>
    ///     The texture pointer.
    /// </summary>
    internal uint ID { get; private set; }

    /// <summary>
    ///     Creates a OpenGL Texture.
    /// </summary>
    public OpenGLTexture(GameGraphics graphics, Image image)
        : base(graphics, image)
    {
        ID    = GL.glGenTexture();
        Image = image;

        GL.glBindTexture(GL.GL_TEXTURE_2D, ID);
        GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, GL.GL_NEAREST);
        GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, GL.GL_NEAREST);
        GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_WRAP_S, GL.GL_CLAMP_TO_BORDER);
        GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_WRAP_T, GL.GL_CLAMP_TO_BORDER);

        unsafe { fixed (Color* ptr = Image.Data)
        {
            GL.glTexImage2D(
                GL.GL_TEXTURE_2D,
                0,
                GL.GL_RGBA,
                Image.Width,
                Image.Height,
                0,
                GL.GL_RGBA,
                GL.GL_UNSIGNED_BYTE,
                (IntPtr)ptr
            );
        }}

        GL.glGenerateMipmap(GL.GL_TEXTURE_2D);
        GL.glBindTexture(GL.GL_TEXTURE_2D, 0u);
    }

    /// <summary>
    ///     Dispose the OpenGL Texture.
    /// </summary>
    protected override void Disposing(bool disposing)
        => GL.glDeleteTexture(ID);

    /// <summary>
    ///     Sets the texture data from the given buffer.
    /// </summary>
    /// <param name="buffer">Buffer to use.</param>
    public override unsafe void SetData<T>(ReadOnlyMemory<T> buffer)
    {
        using MemoryHandle handle = buffer.Pin();
        
        GL.glActiveTexture(GL.GL_TEXTURE0);
        GL.glBindTexture(GL.GL_TEXTURE_2D, ID);
        GL.glTexImage2D(
            GL.GL_TEXTURE_2D,
            0,
            GL.GL_RGBA,
            Image.Width,
            Image.Height,
            0,
            GL.GL_RGBA,
            GL.GL_UNSIGNED_BYTE,
            new IntPtr(handle.Pointer)
        );

        GL.glGenerateMipmap(GL.GL_TEXTURE_2D);
        GL.glBindTexture(GL.GL_TEXTURE_2D, 0u);
    }

    /// <summary>
    ///     Writes the texture to the given buffer.
    /// </summary>
    /// <param name="buffer">Buffer to write.</param>
    public override unsafe void GetData<T>(Memory<T> buffer)
    {
        using var handle = buffer.Pin();

        GL.glActiveTexture(GL.GL_TEXTURE0);
        GL.glBindTexture(GL.GL_TEXTURE_2D, ID);
        GL.glGetTexImage(
            GL.GL_TEXTURE_2D,
            0,
            GL.GL_RGBA,
            GL.GL_UNSIGNED_BYTE,
            new IntPtr(handle.Pointer)
        );
        GL.glBindTexture(GL.GL_TEXTURE_2D, 0u);
    }
}