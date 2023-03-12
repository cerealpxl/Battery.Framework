using System.Runtime.CompilerServices;

namespace Battery.Framework;

/// <summary>
///     A mesh that store vertices used when rendering to the screen.
/// </summary>
public abstract class Mesh<T> : Graphic where T : struct, IVertex
{
    /// <summary>
    ///     Number of vertices in the Mesh.
    /// </summary>
    public int VertexCount { get; private set; }

    /// <summary>
    ///     Number of indices in the Mesh.
    /// </summary>
    public int IndexCount { get; private set; }

    // Array that store vertices.
    internal T[] _vertices;

    // Array that store indices.
    internal uint[] _indices;

    // Whether the mesh has been modified.
    protected bool _dirty = true;
    
    /// <summary>
    ///     Creates a new Mesh.
    /// </summary>
    /// <param name="graphics">The Graphics Backend to which this graphic belongs to.</param>
    /// <param name="capacity">The initial capacity of the mesh.</param>
    public Mesh(GameGraphics graphics, int capacity = 4)
        : base(graphics)
    {
        // Assign variables.
        _vertices   = new T[capacity];
        _indices    = new uint[capacity];
        VertexCount = 0;
        IndexCount  = 0;
    }

    /// <summary>
    ///     Bind this mesh.
    /// </summary>
    public abstract void Bind();

    /// <summary>
    ///     Clears the mesh.
    /// </summary>
    public void Clear()
    {
        IndexCount  = 0;
        VertexCount = 0;
    }

    /// <summary>
    ///     Expands the Array when its capacity is exceed.
    /// </summary>
    private void EnsureCapacity<U>(ref U[] array, int capacity)
    {
        while (capacity > array.Length)
            Array.Resize(ref array, array.Length * 2);

        _dirty = true;
    }

    /// <summary>
    ///     Add vertices to the Mesh.
    /// </summary>
    /// <param name="_vertices">The vertices to add.</param>
    public void AddVertices(T[] _vertices)
    {
        EnsureCapacity(ref this._vertices, VertexCount + _vertices.Length);

        for (int i = 0; i < _vertices.Length; i ++)
            this._vertices[VertexCount ++] = _vertices[i];
    }

    /// <summary>
    ///     Add a vertex to the Mesh.
    /// </summary>
    /// <param name="T">The vertex to add.</param>
    public void AddVertex(T value)
    {
        EnsureCapacity(ref _vertices, VertexCount + 1);
        _vertices[VertexCount ++] = value;
    }

    /// <summary>
    ///     Add two vertices to the Mesh.
    /// </summary>
    /// <param name="a">The first vertex to add.</param>
    /// <param name="b">The second vertex to add.</param>
    public void AddVertices(T a, T b)
    {
        EnsureCapacity(ref _vertices, VertexCount + 2);
        _vertices[VertexCount ++] = a;
        _vertices[VertexCount ++] = b;
    }

    /// <summary>
    ///     Add three vertices to the Mesh.
    /// </summary>
    /// <param name="a">The first vertex to add.</param>
    /// <param name="b">The second vertex to add.</param>
    /// <param name="c">The third vertex to add.</param>
    public void AddVertices(T a, T b, T c) 
    {
        EnsureCapacity(ref _vertices, VertexCount + 3);
        _vertices[VertexCount ++] = a;
        _vertices[VertexCount ++] = b;
        _vertices[VertexCount ++] = c;
    }
    
    /// <summary>
    ///     Add four vertices to the Mesh.
    /// </summary>
    /// <param name="a">The first vertex to add.</param>
    /// <param name="b">The second vertex to add.</param>
    /// <param name="c">The third vertex to add.</param>
    /// <param name="d">The fourth vertex to add.</param>
    public void AddVertices(T a, T b, T c, T d)
    {
        EnsureCapacity(ref _vertices, VertexCount + 4);
        _vertices[VertexCount ++] = a; 
        _vertices[VertexCount ++] = b;
        _vertices[VertexCount ++] = c;
        _vertices[VertexCount ++] = d;
    }

    /// <summary>
    ///     Add indices to the Mesh.
    /// </summary>
    /// <param name="_indices">The indices to add.</param>
    public void AddIndices(uint[] _indices)
    {
        EnsureCapacity(ref this._indices, IndexCount + _indices.Length);

        for (int i = 0; i < _indices.Length; i ++)
            this._indices[IndexCount ++] = _indices[i];
    }

    /// <summary>
    ///     Add a index to the Mesh.
    /// </summary>
    /// <param name="value">The index to add.</param>
    public void AddIndex(uint value)
    {
        EnsureCapacity(ref _indices, IndexCount + 1);
        _indices[IndexCount ++] = value;
    }

    /// <summary>
    ///     Add two indices to the Mesh.
    /// </summary>
    /// <param name="a">The first index to add.</param>
    /// <param name="b">The second index to add.</param>
    public void AddIndices(uint a, uint b)
    {
        EnsureCapacity(ref _indices, IndexCount + 2);
        _indices[IndexCount ++] = a;
        _indices[IndexCount ++] = b;
    }

    /// <summary>
    ///     Add three indices to the Mesh.
    /// </summary>
    /// <param name="a">The first index to add.</param>
    /// <param name="b">The second index to add.</param>
    /// <param name="c">The third index to add.</param>
    public void AddIndices(uint a, uint b, uint c)
    {
        EnsureCapacity(ref _indices, IndexCount + 3);
        _indices[IndexCount ++] = a;
        _indices[IndexCount ++] = b;
        _indices[IndexCount ++] = c;
    }

    /// <summary>
    ///     Add four indices to the Mesh.
    /// </summary>
    /// <param name="a">The first index to add.</param>
    /// <param name="b">The second index to add.</param>
    /// <param name="c">The third index to add.</param>
    /// <param name="d">The fourth index to add.</param>
    public void AddIndices(uint a, uint b, uint c, uint d)
    {
        EnsureCapacity(ref _indices, IndexCount + 4);
        _indices[IndexCount ++] = a;
        _indices[IndexCount ++] = b;
        _indices[IndexCount ++] = c;
        _indices[IndexCount ++] = d;
    }

    /// <summary>
    ///     Add a triangle to the Mesh.
    /// </summary>
    /// <param name="a">The first vertex to add.</param>
    /// <param name="b">The second vertex to add.</param>
    /// <param name="c">The third vertex to add.</param>
    public void AddTriangle(ref T a, ref T b, ref T c)
    {
        uint count = (uint)VertexCount;
        EnsureCapacity(ref _indices, IndexCount + 3);
        _indices[IndexCount ++] = count;
        _indices[IndexCount ++] = count + 1;
        _indices[IndexCount ++] = count + 2;

        EnsureCapacity(ref _vertices, VertexCount + 3);
        _vertices[VertexCount ++] = a;
        _vertices[VertexCount ++] = b;
        _vertices[VertexCount ++] = c;
    }

    /// <summary>
    ///     Add a triangle to the Mesh.
    /// </summary>
    /// <param name="a">The first vertex to add.</param>
    /// <param name="b">The second vertex to add.</param>
    /// <param name="c">The third vertex to add.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddTriangle(T a, T b, T c)
    {
        AddTriangle(ref a, ref b, ref c);
    }

    /// <summary>
    ///     Add a quad to the Mesh.
    /// </summary>
    /// <param name="a">The first vertex to add.</param>
    /// <param name="b">The second vertex to add.</param>
    /// <param name="c">The third vertex to add.</param>
    /// <param name="d">The fourth vertex to add.</param>
    public void AddQuad(ref T a, ref T b, ref T c, ref T d)
    {
        uint count = (uint)VertexCount;
        EnsureCapacity(ref _indices, IndexCount + 6);
        _indices[IndexCount ++] = count;
        _indices[IndexCount ++] = count + 1;
        _indices[IndexCount ++] = count + 2;
        _indices[IndexCount ++] = count;
        _indices[IndexCount ++] = count + 2;
        _indices[IndexCount ++] = count + 3;

        EnsureCapacity(ref _vertices, VertexCount + 4);
        _vertices[VertexCount ++] = a;
        _vertices[VertexCount ++] = b;
        _vertices[VertexCount ++] = c;
        _vertices[VertexCount ++] = d;
    }

    /// <summary>
    ///     Add a quad to the Mesh.
    /// </summary>
    /// <param name="a">The first vertex to add.</param>
    /// <param name="b">The second vertex to add.</param>
    /// <param name="c">The third vertex to add.</param>
    /// <param name="d">The fourth vertex to add.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddQuad(T a, T b, T c, T d)
    {
        AddQuad(ref a, ref b, ref c, ref d);
    }
    
    /// <summary>
    ///     Add a geometry to the Mesh.
    /// </summary>
    /// <param name="vertices">The vertices to add.</param>
    /// <param name="indices">The indices to add.</param>
    public void AddGeometry(T[] vertices, uint[] indices)
    {
        uint count = (uint)VertexCount;
        EnsureCapacity(ref _indices, IndexCount + indices.Length);

        for (int i = 0; i < indices.Length; i ++)
            _indices[IndexCount ++] = count + indices[i];

        EnsureCapacity(ref _vertices, VertexCount + _vertices.Length);
        for (int j = 0; j < vertices.Length; j ++)
            _vertices[VertexCount ++] = vertices[j];
    }
}