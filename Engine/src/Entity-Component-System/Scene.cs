using Battery.Framework;

namespace Battery.Engine;

/// <summary>
///     A container for Component Systems and Entities.
/// </summary>
public class Scene
{
    /// <summary>
    ///     Whether the Scene has been started.
    /// </summary>
    public bool Started = false;
    
    /// <summary>
    ///     Whether the Scene is active. 
    ///     If true, the Update events will be executed.
    /// </summary>
    public bool Active = true;

    /// <summary>
    ///     Whether the Scene is visible.
    ///     If true, the Render events will be executed.
    /// </summary>
    public bool Visible = true;

    /// <summary>
    ///     The table storing all the entities in this Scene.
    /// </summary>
    public Table<Entity> Entities = new Table<Entity>();

    /// <summary>
    ///     The table storing all the component systems in this Scene.
    /// </summary>
    public Table<ComponentSystem> Systems = new Table<ComponentSystem>();

    /// <summary>
    ///     Execute all the Begin callbacks of the Component Systems.
    /// </summary>
    public virtual void Begin()
    {
        Started = true;

        foreach (var system in Systems)
            system.Begin();
    }

    /// <summary>
    ///     Execute all the End callbacks of the Component Systems.
    /// </summary>
    public virtual void End()
    {
        foreach (var system in Systems)
            system.End();

        Started = false;
    }

    /// <summary>
    ///     Execute all the Update callbacks of the Component Systems.
    /// </summary>
    public virtual void Update()
    {
        foreach (var system in Systems)
        {
            if (system.Active)
                system.Update();
        }
    }

    /// <summary>
    ///     Execute all the Render Begin callbacks, before the Render method.
    /// </summary>
    public virtual void RenderBegin()
    {
        foreach (var system in Systems)
        {
            if (system.Visible)
                system.RenderBegin();
        }
    }

    /// <summary>
    ///     Execute all the Render callbacks of the Component Systems.
    /// </summary>
    public virtual void Render()
    {
        foreach (var system in Systems)
        {
            if (system.Visible)
                system.Render();
        }
    }

    /// <summary>
    ///     Execute all the Render End callbacks, after the Render method.
    /// </summary>
    public virtual void RenderEnd()
    {
        foreach (var system in Systems)
        {
            if (system.Visible)
                system.RenderEnd();
        }
    }

    #region Components

    /// <summary>
    ///     Add a component to the Systems.
    /// </summary>
    /// <param name="component">The component to add.</param>
    internal void AddComponent(Component component)
    {
        foreach (var system in Systems)
            system.Add(component);
    }

    /// <summary>
    ///     Remove a component from the Systems.
    /// </summary>
    /// <param name="component">The component to remove.</param>
    internal void RemoveComponent(Component component)
    {
        foreach (var system in Systems)
            system.Remove(component);
    }

    #endregion

    #region Component Systems

    /// <summary>
    ///     Adds a <see cref="ComponentSystem" /> to the Scene.
    /// </summary>
    /// <param name="system">The system to add.</param>
    public T AddSystem<T>(T system) where T : ComponentSystem
    {
        if (system.Scene != null)
            throw new Exception("The system has already been added to a Scene.");

        // Add the systems from the table.
        Systems.Add(system);

        // Add all the entity components to the system.
        foreach (var entity in Entities)
        {
            foreach (var component in entity)
                system.Add(component);
        }

        // Assign the variables and starts the system.
        system.Scene = this;
        system.Added();

        if (Started)
            system.Begin();

        return system;
    }

    /// <summary>
    ///     Adds a <see cref="ComponentSystem" /> to the Scene.
    /// </summary>
    public T AddSystem<T>() where T : ComponentSystem, new()
        => AddSystem(new T());

    /// <summary>
    ///     Gets a <see cref="ComponentSystem" /> that matches the given type.
    /// </summary>
    public T? GetSystem<T>()
        => Systems.Get<T>();

    /// <summary>
    ///     Check if the scene has an <see cref="ComponentSystem" /> that matches the given type.
    /// </summary>
    public bool HasSystem<T>()
        => GetSystem<T>() != null;

    /// <summary>
    ///     Removes a <see cref="ComponentSystem" /> from the Scene.
    /// </summary>
    /// <param name="system">The system to remove.</param>
    public T RemoveSystem<T>(T system) where T : ComponentSystem
    {
        if (system.Scene == null)
            throw new Exception("The system is not in this Scene.");

        // Assign the variables and ends the system.
        if (Started)
            system.End();

        system.Removed();
        system.Scene = null;

        // Removes the systems from the table.
        Systems.Remove(system);

        return system;
    }

    #endregion

    #region Entities

    /// <summary>
    ///     Adds a <see cref="Entity" /> to the Scene.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    public T AddEntity<T>(T entity) where T : Entity
    {
        if (entity.Scene != null)
            throw new Exception("The entity has already been added to a Scene.");

        Entities.Add(entity);
        entity.Scene = this;
        entity.Added();

        // Loop the entity components and add them to systems.
        foreach (var component in entity)
            AddComponent(component);
        
        return entity;
    }
    /// <summary>
    ///     Adds a <see cref="Entity" /> to the Scene.
    /// </summary>
    public T AddEntity<T>() where T : Entity, new()
        => AddEntity(new T());


    /// <summary>
    ///     Gets a <see cref="Entity" /> that matches the given type in the sSene.
    /// </summary>
    public T? GetEntity<T>()
        => Entities.Get<T>();

    /// <summary>
    ///     Check if the scene has an <see cref="Entity" /> that matches the given type.
    /// </summary>
    public bool HasEntity<T>()
        => GetEntity<T>() != null;

    /// <summary>
    ///     Removes a <see cref="Entity" /> from the Scene.
    /// </summary>
    /// <param name="entity">The entity to remove.</param>
    public T RemoveEntity<T>(T entity) where T : Entity
    {
        if (entity.Scene == null)
            throw new Exception("The entity is not in this Scene.");
        
        // Loop the entity components and remove them to systems.
        foreach (var component in entity)
            RemoveComponent(component);

        entity.Removed();
        entity.Scene = null;
        Entities.Remove(entity);

        return entity;
    }

    #endregion
}