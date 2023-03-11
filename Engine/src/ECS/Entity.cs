using System.Collections;
using Battery.Framework;

namespace Battery.Engine;

/// <summary>
///     A container for Components.
/// </summary>
public class Entity : IEnumerable<Component>
{
    /// <summary>
    ///     The identifier of the Entity.
    /// </summary>
    public Guid Guid = Guid.NewGuid();

    /// <summary>
    ///     The <see cref="Battery.Engine.Scene"/> to which the entity belongs to.
    /// </summary>
    public Scene? Scene;

    /// <summary>
    ///     Whether the entity is active. If true, the systems will call the Update methods of the Components.
    /// </summary>
    public bool Active = true;

    /// <summary>
    ///     Whether the entity is visible. If true, the systems will call the Render methods of the Components.
    /// </summary>
    public bool Visible = true;

    /// <summary>
    ///     A table storing all the components.
    /// </summary>
    public Table<Component> Components = new Table<Component>();
    
    /// <summary>
    ///     Creates a new instance of the <see cref="Entity" /> class.
    /// </summary>
    /// <param name="components">Some default components.</param>
    public Entity(params Component[] components)
    {
        foreach (var component in components)
            Add(component);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="Entity" /> class.
    /// </summary>
    public Entity()
    {
    }

    /// <summary>
    ///     Called when added to the Scene.
    /// </summary>    
    public virtual void Added()
    {
    }

    /// <summary>
    ///     Called when removevd from the Scene.
    /// </summary>
    public virtual void Removed()
    {
    }

    /// <summary>
    ///     Adds a <see cref="Component" /> to the Entity.
    /// </summary>
    /// <param name="component">The component to add.</param>
    public T Add<T>(T component) where T : Component
    {
        Components.Add(component);
        component.Entity = this;
        component.OnAdd?.Invoke();

        Scene?.AddComponent(component);
        return component;
    }

    /// <summary>
    ///     Gets a <see cref="Component" /> in the entity that matches the given type.
    /// </summary>
    public T? Get<T>()
        => Components.Get<T>();

    /// <summary>
    ///     Checks if the entity has a <see cref="Component" /> that matches the given type.
    /// </summary>
    public bool Has<T>()
        => Get<T>() != null;

    /// <summary>
    ///     Removes a <see cref="Component" /> from the Entity.
    /// </summary>
    /// <param name="component">The component to remove.</param>
    public T Remove<T>(T component) where T : Component
    {
        Scene?.RemoveComponent(component);
        
        component.OnRemove?.Invoke();
        component.Entity = null;
        Components.Remove(component);

        return component;
    }

    /// <summary>
    ///     Removes this entity from the Scene.
    /// </summary>
    public void RemoveThis()
        => Scene?.RemoveEntity(this);
    
    /// <summary>
    ///     Returns the IEnumerator function.
    /// </summary>
    public IEnumerator<Component> GetEnumerator()
        => Components.GetEnumerator();

    /// <summary>
    ///     IEnumerator function. Used in foreach.
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator() 
        => GetEnumerator();
}