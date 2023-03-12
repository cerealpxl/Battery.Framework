namespace Battery.Framework;

/// <summary>
///     Hold values that define the state of a Shader, used for rendering.
/// </summary>
public class ShaderMaterial
{
    /// <summary>
    ///     The shader assigned to the Material.
    /// </summary>
    public Shader Shader { get; private set; }
    
    // List that store the uniforms of the Shader.
    internal Dictionary<string, object?> _uniforms = new Dictionary<string, object?>();

    /// <summary>
    ///     Creates a new instance of the <see cref="ShaderMaterial" /> class.
    /// </summary>
    /// <param name="shader">The shader to use.</param>
    public ShaderMaterial(Shader shader)
    {
        Shader = shader;
    }

    /// <summary>
    ///     Adds a uniform to the Shader Material.
    /// </summary>
    /// <param name="name">The name of the uniform.</param>
    /// <param name="value">The value of the uniform.</param>
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
    ///     Gets a uniform in the ShaderMaterial.
    /// </summary>
    /// <param name="name">The name of the uniform.</param>
    public object? GetUniform(string name)
    {
        _uniforms.TryGetValue(name, out var result);
        return result;
    }
}