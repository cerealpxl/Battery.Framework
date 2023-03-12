namespace Battery.Framework;

/// <summary>
///     Represents a Shader Program, containing a Vertex and Fragment Shader.
///     Used during rendering.
/// </summary>
public abstract class Shader : IDisposable
{
    /// <summary>
    ///     Creates a new <see cref="Shader"/> using the given vertex and fragment.
    /// </summary>
    /// <param name="source">A tuple containing the Vertex and Fragment sources, respectively.</param>
    public Shader((string, string) source)
    {
    }

    /// <summary>
    ///     Dispose the Shader program.
    /// </summary>
    public abstract void Dispose();
}