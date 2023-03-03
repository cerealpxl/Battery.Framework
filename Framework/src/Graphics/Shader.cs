namespace Battery.Framework;

/// <summary>
///     Structure that handle the Shader Programs logic.
/// </summary>
public abstract class Shader : Graphic
{
    /// <summary>
    ///     Creates a new shader using the given vertex and fragment.
    /// </summary>
    /// <param name="vertexSource">The source code of the vertex shader.</param>
    /// <param name="fragmentSource">The source code of the fragment shader.</param>
    public Shader(string vertexSource, string fragmentSource)
    {
    }

    /// <summary>
    ///     Use this shader program.
    /// </summary>
    public abstract void Bind();
    
    /// <summary>
    ///     Use this shader program.
    /// </summary>
    public abstract void Unbind();
}