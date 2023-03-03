using System.Numerics;
using OpenGL;

namespace Battery.Framework;

/// <summary>
///     A structure used to pass the shader state to a Render Call.
/// </summary>
public class ShaderMaterial
{
    /// <summary>
    ///     Shader to use.
    /// </summary>
    public Shader Shader { get; private set; }
    
    // List that store the uniforms of the Shader.
    internal Dictionary<string, object?> _uniforms = new Dictionary<string, object?>();

    /// <summary>
    ///     Creates a new instance of the <see cref="ShaderMaterial" /> class.
    /// </summary>
    /// <param name="shader">Shader to use.</param>
    public ShaderMaterial(Shader shader)
    {
        Shader = shader;
    }

    /// <summary>
    /// Add a uniform to the ShaderMaterial.
    /// </summary>
    /// <param name="name">The name of the uniform.</param>
    /// <param name="value">The ShaderMaterial of the uniform.</param>
    public void SetUniform(string name, object? value)
    {
        if (_uniforms.ContainsKey(name))
        {
            _uniforms[name] = value;
            return;
        }

        _uniforms.Add(name, value);
    }

    /// <summary>
    /// Gets a uniform in the ShaderMaterial.
    /// </summary>
    /// <param name="name">The name of the uniform.</param>
    public object? GetUniform(string name)
    {
        _uniforms.TryGetValue(name, out var result);
        return result;
    }
}