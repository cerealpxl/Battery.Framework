namespace Battery.Framework;

/// <summary>
///     Game Manager, used to setup callbacks of the Game Events.
/// </summary>
public abstract class GameManager : IComparable<GameManager>
{
    /// <summary>
    ///     Whether the Update methods can be called.
    /// </summary>
    public bool Active = true;

    /// <summary>
    ///     Whether the Render methods can be called.
    /// </summary>
    public bool Visible = true;

    /// <summary>
    ///     The priority of the manager in the order of callbacks.
    /// </summary>
    public int Priority = 1000;

    /// <summary>
    ///     Called when the Game begins.
    ///     If the Game has already started, this will be called immediately.
    /// </summary>
    public virtual void Begin()
    {
    }

    /// <summary>
    ///     Called when the Game ends.
    ///     If the Game has already started, this will be called immediately.
    /// </summary>
    public virtual void End()
    {
    }

    /// <summary>
    ///     Called in every fixed update of the Game.
    /// </summary>
    public virtual void Update()
    {
    }

    /// <summary>
    ///     Called before any rendering.
    /// </summary>
    public virtual void RenderBegin()
    {
    }

    /// <summary>
    ///     Called to render the screen content.
    /// </summary>
    public virtual void Render()
    {
    }

    /// <summary>
    ///     Called after all the rendering.
    /// </summary>
    public virtual void RenderEnd()
    {
    }

    /// <summary>
    ///     Comparison method.
    /// </summary>
    /// <param name="other">The other manager to compare.</param>
    int IComparable<GameManager>.CompareTo(GameManager? other)
        => (other == null ? 0 : other.Priority) - this.Priority;
}