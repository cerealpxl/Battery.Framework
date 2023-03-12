using OpenGL;

namespace Battery.Framework;

/// <summary>
///     Structure that handle the Shader Programs logic.
/// </summary>
public class OpenGLShader : Shader
{
    /// <summary>
    ///     ID of the shader program.
    /// </summary>
    internal uint _id { get; private set; }

    /// <summary>
    ///     Creates a new shader using the given vertex and fragment.
    /// </summary>
    /// <param name="vertexSource">The source code of the vertex shader.</param>
    /// <param name="fragmentSource">The source code of the fragment shader.</param>
    public OpenGLShader(GameGraphics graphics, string vertexSource, string fragmentSource)
        : base(graphics, vertexSource, fragmentSource)
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
        var vertex   = CreateShader(GL.GL_VERTEX_SHADER,   vertexSource);
        var fragment = CreateShader(GL.GL_FRAGMENT_SHADER, fragmentSource);

        // Create the program and link the shaders to it.
        _id = GL.glCreateProgram();
        GL.glAttachShader(_id, vertex);
        GL.glAttachShader(_id, fragment);
        
        GL.glLinkProgram(_id);

        CheckError("Error while linking the shader program: ", GL.glGetProgramInfoLog(_id));
        GL.glDeleteShader(vertex);
        GL.glDeleteShader(fragment);
    }

    /// <summary>
    ///     Dispose this shader program.
    /// </summary>
    protected override void Disposing(bool disposing) => GL.glDeleteProgram(_id);

    /// <summary>
    ///     Use this shader program.
    /// </summary>
    public override void Bind() => GL.glUseProgram(_id);

    /// <summary>
    ///     Unbind the shader program.
    /// </summary>
    public override void Unbind() => GL.glUseProgram(0u);
}