using System.Runtime.InteropServices;
using OpenGL;

namespace Battery.Framework;

/// <summary>
///     Represents a single OpenGL Mesh.
/// </summary>
public class OpenGLMesh<T> : Mesh<T> where T : struct, IVertex
{   
    // Vertex Array ID.
    internal uint _vertexArrayID;
    
    // Vertex Buffer ID.
    internal uint _vertexBufferID;

    // Index Buffer ID.
    internal uint _indexBufferID;

    /// <summary>
    ///     Creates a new instance of <see cref="OpenGLMesh"/> class.
    /// </summary>
    /// <param name="capacity">The initial capacity of the mesh.</param>
    public OpenGLMesh(int capacity = 4)
    {
        // Creates the mesh implementation.
        _vertexArrayID  = GL.glGenVertexArray();
        _vertexBufferID = GL.glGenBuffer();
        _indexBufferID  = GL.glGenBuffer();
    }

    /// <summary>
    ///     Delete the OpenGL Mesh by deleting the Vertex Array and its Buffers.
    /// </summary>
    public override void Dispose()
    {
        GL.glDeleteBuffer(_vertexBufferID);
        GL.glDeleteBuffer(_indexBufferID);
        GL.glDeleteVertexArray(_vertexArrayID);
    }

    /// <inheritdoc/>
    public override void Bind()
    {
        if (_dirty)
            Upload();

        GL.glBindVertexArray(_vertexArrayID);
        GL.glBindBuffer(GL.GL_ARRAY_BUFFER, _vertexBufferID);
        GL.glBindBuffer(GL.GL_ELEMENT_ARRAY_BUFFER, _indexBufferID);
    }

    /// <summary>
    ///     Upload the Mesh elements to the GPU.
    /// </summary>
    internal unsafe void Upload()
    {
        var size = Marshal.SizeOf<T>();

        GL.glBindVertexArray(_vertexArrayID);
        GL.glBindBuffer(GL.GL_ARRAY_BUFFER, _vertexBufferID);
        GL.glBindBuffer(GL.GL_ELEMENT_ARRAY_BUFFER, _indexBufferID);

        using var pinned1 = _vertices.AsMemory().Pin();
        GL.glBufferData(GL.GL_ARRAY_BUFFER, _vertices.Count() * size, pinned1.Pointer, GL.GL_DYNAMIC_DRAW);

        using var pinned2 = _indices.AsMemory().Pin();
        GL.glBufferData(GL.GL_ELEMENT_ARRAY_BUFFER, _indices.Count() * sizeof(uint), pinned2.Pointer, GL.GL_DYNAMIC_DRAW);
        
        GL.glBindBuffer(GL.GL_ARRAY_BUFFER, 0u);
        GL.glBindBuffer(GL.GL_ELEMENT_ARRAY_BUFFER, 0u);
        GL.glBindVertexArray(0u);

        _dirty = false;
    }
}