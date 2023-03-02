using System.Runtime.InteropServices;
using OpenGL;

namespace Battery.Framework;

/// <summary>
///     A OpenGL mesh.
/// </summary>
public class OpenGLMesh : Mesh
{   
    /// <summary>
    ///     Vertex Array ID.
    /// </summary>
    internal uint _vertexArrayID;
    
    /// <summary>
    ///     Vertex Buffer ID.
    /// </summary>
    internal uint _vertexBufferID;

    /// <summary>
    ///     Index Buffer ID.
    /// </summary>
    internal uint _indexBufferID;

    /// <summary>
    ///     Creates a new OpenGL mesh.
    /// </summary>
    /// <param name="capacity">The initial capacity of the mesh.</param>
    public OpenGLMesh(int capacity = 4)
        : base(capacity)
    {
        // Creates the mesh implementation.
        _vertexArrayID  = GL.glGenVertexArray();
        _vertexBufferID = GL.glGenBuffer();
        _indexBufferID  = GL.glGenBuffer();
    }

    /// <summary>
    ///     Dispose the OpenGL mesh.
    /// </summary>
    protected override void Disposing(bool disposing)
    {
        GL.glDeleteBuffer(_vertexBufferID);
        GL.glDeleteBuffer(_indexBufferID);
        GL.glDeleteVertexArray(_vertexArrayID);
    }

    /// <summary>
    ///     Bind this mesh.
    /// </summary>
    public override void Bind()
    {
        if (_dirty)
            Upload();

        GL.glBindVertexArray(_vertexArrayID);
        GL.glBindBuffer(GL.GL_ARRAY_BUFFER, _vertexBufferID);
        GL.glBindBuffer(GL.GL_ELEMENT_ARRAY_BUFFER, _indexBufferID);
    }

    /// <summary>
    ///     Upload the mesh elements.
    /// </summary>
    internal unsafe void Upload()
    {
        GL.glBindVertexArray(_vertexArrayID);
        GL.glBindBuffer(GL.GL_ARRAY_BUFFER, _vertexBufferID);
        GL.glBindBuffer(GL.GL_ELEMENT_ARRAY_BUFFER, _indexBufferID);

        var size = Marshal.SizeOf<Vertex>();

        fixed (Vertex* pointer = &_vertices[0])
            GL.glBufferData(GL.GL_ARRAY_BUFFER, _vertices.Count() * size, pointer, GL.GL_DYNAMIC_DRAW);

        fixed (uint* pointer = &_indices[0])
            GL.glBufferData(GL.GL_ELEMENT_ARRAY_BUFFER, _indices.Count() * sizeof(uint), pointer, GL.GL_DYNAMIC_DRAW);
        
        GL.glBindBuffer(GL.GL_ARRAY_BUFFER, 0u);
        GL.glBindBuffer(GL.GL_ELEMENT_ARRAY_BUFFER, 0u);
        GL.glBindVertexArray(0u);

        _dirty = false;
    }
}