namespace Battery.Framework;

/// <summary>
///     Implements the platform functions for the OpenGL Backend.
/// </summary>
public interface IPlatformWithOpenGL
{
    /// <summary>
    ///     Class that extends the OpenGL context.
    /// </summary>
    public abstract class Context : IDisposable
    {
        /// <summary>
        ///     Dispose the OpenGL context.
        /// </summary>
        public abstract void Dispose();
    }

    /// <summary>
    ///     Gets the adress of a single OpenGL function.
    /// </summary>
    /// <param name="name">The name of the function.</param>
    IntPtr GetGLProcAdress(string name);

    /// <summary>
    ///     Creates the OpenGL Context.
    /// </summary>
    Context CreateContext();

    /// <summary>
    ///     Gets the current OpenGL Context.
    /// </summary>
    Context GetContext();

    /// <summary>
    ///     Sets the current OpenGL Context.
    /// </summary>
    /// <param name="context">The context to set.</param>
    Context SetContext(Context context);
}