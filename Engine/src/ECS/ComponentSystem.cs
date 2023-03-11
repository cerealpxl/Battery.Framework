using Battery.Framework;

namespace Battery.Engine;

/// <summary>
///     Provides the logic of the Components.
/// </summary>
public abstract class ComponentSystem : ITagged
{
    /// <summary>
    ///     The <see cref="Battery.Engine.Scene"/> to which the system belongs.
    /// </summary>
    public Scene? Scene;

    /// <summary>
    ///     Whether the System is active. If true, calls Update methods.
    /// </summary>
    public bool Active;

    /// <summary>
    ///     Whether the System is visible. If true, calls the Render methods.
    /// </summary>
    public bool Visible;

    /// <summary>
    ///     The tags used by the system.
    /// </summary>
    public Tag Tag { get; set; } = Tag.All;

    /// <summary>
    ///     Creates a System that handle the given Type.
    /// </summary>
    /// <param name="active">Whether the system is active by default.</param>
    /// <param name="visible">Whether the system is visible by default.</param>
    protected ComponentSystem(bool active = false, bool visible = false)
    {
        Active  = active;
        Visible = visible;
    }

    /// <summary>
    ///     Called when added to a <see cref="Battery.Engine.Scene"/>.
    /// </summary>
    public virtual void Added()
    {
    }

    /// <summary>
    ///     Called when removed from a <see cref="Battery.Engine.Scene"/>.
    /// </summary>
    public virtual void Removed()
    {
    }

    /// <summary>
    ///     Called when the <see cref="Battery.Engine.Scene"/> begins or when added after the <see cref="Battery.Engine.Scene"/> begins.
    /// </summary>
    public virtual void Begin()
    {
    }

    /// <summary>
    ///     Called when the <see cref="Battery.Engine.Scene"/> ends or when removed after the <see cref="Battery.Engine.Scene"/> begins.
    /// </summary>
    public virtual void End()
    {
    }

    /// <summary>
    ///     Called in every fixed timestep of the <see cref="Battery.Engine.Scene"/>.
    /// </summary>
    /// <param name="time">The <see cref="GameTime" /> struct.</param>
    public virtual void Update(GameTime time)
    {
    }


    /// <summary>
    ///     Called before the Render method.
    /// </summary>
    /// <param name="time">The <see cref="GameTime" /> struct.</param>
    public virtual void RenderBegin(GameTime time)
    {
    }

    /// <summary>
    ///     Called to render the system to the screen.
    /// </summary>
    /// <param name="time">The <see cref="GameTime" /> struct.</param>
    public virtual void Render(GameTime time)
    {
    }

    /// <summary>
    ///     Called after the Render method.
    /// </summary>
    /// <param name="time">The <see cref="GameTime" /> struct.</param>
    public virtual void RenderEnd(GameTime time)
    {
    }

    /// <summary>
    ///     Add a <see cref="Component"/> to the system.
    /// </summary>
    /// <param name="component">The component to add.</param>
    public virtual void Add(Component component)
    {
    }   

    /// <summary>
    ///     Remove a <see cref="Component"/> from the system.
    /// </summary>
    /// <param name="component">The component to remove.</param>
    public virtual void Remove(Component component)
    {
    }
}