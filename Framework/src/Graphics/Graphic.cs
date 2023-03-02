namespace Battery.Framework;

/// <summary>
///     Class that manages the disposing of the graphics resource.
/// </summary>
public abstract class Graphic : IDisposable
{
    /// <summary>
    ///     Whether the resource has been disposed.
    /// </summary>
    public bool Disposed { get; private set; }

    // A list storing the resources.
    private static HashSet<Graphic> _resources = new HashSet<Graphic>();

    /// <summary>
    ///     Creates a new graphics resource.
    /// </summary>
    public Graphic()
    {
        _resources.Add(this);
    }
    
    ~Graphic()
    {
        Dispose(disposing: false);
    }

    /// <summary>
    ///     Dispose this resource.
    /// </summary>
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Dispose(disposing: true);
    }

    /// <summary>
    ///     Dispose the graphics resource.
    /// </summary>
    /// <param name="disposing">Whether is disposing by calling the function.</param>
    private void Dispose(bool disposing)
    {
        if (!Disposed)
        {
            _resources.Remove(this);
            
            Disposed = true;
            Disposing(disposing);
        }
    }
    
    /// <summary>
    ///     Dispose all resources.
    /// </summary>
	public static void DisposeAll()
	{
		List<Graphic> list = new List<Graphic>(_resources);

		for (int i = 0; i < list.Count; i++)
			list[i].Dispose();

		list.Clear();
	}

    /// <summary>
    ///     Function called when disposing this resource.
    /// </summary>
    /// <param name="disposing">Whether is disposing by calling the function.</param>
	protected abstract void Disposing(bool disposing);
}