namespace Battery.Framework;

/// <summary>
///     Game Manager, used to setup callbacks of the Game Events.
/// </summary>
public abstract class GameManager
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
    public virtual void Update(GameTime time)
    {
    }

    /// <summary>
    ///     Called before any rendering.
    /// </summary>
    public virtual void RenderBegin(GameTime time)
    {
    }

    /// <summary>
    ///     Called to render the screen content.
    /// </summary>
    public virtual void Render(GameTime time)
    {
    }

    /// <summary>
    ///     Called after all the rendering.
    /// </summary>
    public virtual void RenderEnd(GameTime time)
    {
    }
}