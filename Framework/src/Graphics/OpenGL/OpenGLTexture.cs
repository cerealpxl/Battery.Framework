using System.Buffers;
using OpenGL;

namespace Battery.Framework;

/// <summary>
///     Represents a single OpenGL Texture.
/// </summary>
public class OpenGLTexture : Texture
{
    /// <summary>
    ///     The pointer to the Texture.
    /// </summary>
    internal uint ID { get; private set; }

    /// <summary>
    ///     Creates a new instance of <see cref="OpenGLTexture"/> class.
    /// </summary>
    public OpenGLTexture(Image image)
        : base(image)
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
    ///     Delete the OpenGL Texture from the GPU.
    /// </summary>
    public override void Dispose()
    {
        GL.glDeleteTexture(ID);
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
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