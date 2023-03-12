using OpenGL;

namespace Battery.Framework;

/// <summary>
///     Represents a single OpenGL Shader Program.
/// </summary>
public class OpenGLShader : Shader
{
    /// <summary>
    ///     The id of the Shader Program.
    /// </summary>
    internal uint _id { get; private set; }

    /// <inheritdoc/>
    public OpenGLShader((string, string) source)
        : base(source)
    {
        /// <summary>
        ///     Throw an error.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="errorMsg"></param>
        static void CheckError(string msg, string? errorMsg)
        {
            if (string.IsNullOrEmpty(errorMsg) == false)
                throw new Exception(msg + errorMsg);
        }

        /// <summary>
        ///     Function used to create and compile a shader.
        /// </summary>
        /// <param name="kind">Type of shader.</param>
        /// <param name="source">Code of shader.</param>
        static uint CreateShader(int kind, string source)
        {
            var shader = GL.glCreateShader(kind);
            GL.glShaderSource(shader, source);
            GL.glCompileShader(shader);

            CheckError("Error while compiling a shader: ", GL.glGetShaderInfoLog(shader));
            return shader;
        }

        // Create the shaders.
        var vertex   = CreateShader(GL.GL_VERTEX_SHADER,   source.Item1);
        var fragment = CreateShader(GL.GL_FRAGMENT_SHADER, source.Item2);

        // Create the program and link the shaders to it.
        _id = GL.glCreateProgram();
        GL.glAttachShader(_id, vertex);
        GL.glAttachShader(_id, fragment);
        
        GL.glLinkProgram(_id);

        CheckError("Error while linking the shader program: ", GL.glGetProgramInfoLog(_id));
        GL.glDeleteShader(vertex);
        GL.glDeleteShader(fragment);
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        GL.glDeleteProgram(_id);
    }
}