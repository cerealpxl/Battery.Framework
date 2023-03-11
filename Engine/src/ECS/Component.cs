namespace Battery.Engine;

/// <summary>
///     Base class for Components.
/// </summary>
public class Component
{
    /// <summary>
    ///     The entity to which the component belongs to.
    /// </summary>
    public Entity? Entity;
    
    /// <summary>
    ///     The scene to which the component belongs to.
    /// </summary>
    public Scene? Scene => Entity == null ? null : Entity.Scene; 

    /// <summary>
    ///     Action called when the component is added to an <see cref="Battery.Engine.Entity" />.
    /// </summary>
    public Action? OnAdd;

    /// <summary>
    ///     Action called when the component is removed from an <see cref="Battery.Engine.Entity" />.
    /// </summary>
    public Action? OnRemove;

    /// <summary>
    ///     Remove this component from the Entity.
    /// </summary>
    public void RemoveThis()
        => Entity?.Remove(this);
}