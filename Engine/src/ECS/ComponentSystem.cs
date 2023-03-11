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
    ///     Do game logic here. Called in every fixed step.
    /// </summary>
    public virtual void Update()
    {
    }

    /// <summary>
    ///     Called when the <see cref="Battery.Engine.Scene"/> ends or when removed after the <see cref="Battery.Engine.Scene"/> begins.
    /// </summary>
    public virtual void End()
    {
    }

    /// <summary>
    ///     Do drawing operations here. Called before the <see cref="Render"/> method.
    /// </summary>
    public virtual void RenderBegin()
    {
    }

    /// <summary>
    ///     Do drawing operations here. Called in the Render state.
    /// </summary>
    public virtual void Render()
    {
    }

    /// <summary>
    ///     Do drawing operations here. Called after the <see cref="Render"/> method.
    /// </summary>
    public virtual void RenderEnd()
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