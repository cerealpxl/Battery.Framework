using System.Numerics;
using Battery.Framework;

namespace Battery.Engine;

/// <summary>
///     Component that provides the logic for Collision Objects.
/// </summary>
public abstract class Collider : Component, ITagged
{
    /// <summary>
    ///     The tags of the Collider.
    /// </summary>
    public Tag Tag { get; set; } = Tag.Default;

    /// <summary>
    ///     Whether the collision check with this collider is enabled.
    /// </summary>
    public bool Collideable = true;

    /// <summary>
    ///     A table storing all the colliders in the Game.
    /// </summary>
    public static Table<Collider> Colliders = new Table<Collider>();
    
    /// <summary>
    ///     Creates a new instance of <see cref="Collider"/> component.
    /// </summary>
    /// <param name="tags"></param>
    public Collider(Tag tags)
    {
        Tag = tags;

        OnAdd    += () => Colliders.Add(this);
        OnRemove += () => Colliders.Remove(this);
    }

    /// <summary>
    ///     Check collision with the given collider.
    /// </summary>
    /// <param name="other">The collider to check.</param>
    public bool Colliding(Collider other)
    {
        // Collision between Box Colliders.
        if (this is BoxCollider box1 && other is BoxCollider box2)
        {
            return box1.Colliding(box2);
        }

        throw new NotImplementedException($"{GetType().Name} X {other.GetType().Name} collision.");
    }

    /// <summary>
    ///     Check collision with the given collider at the specified position.
    /// </summary>
    /// <param name="other">The collider to check.</param>
    /// <param name="position">The position to check.</param>
    public bool Colliding(Collider other, Vector2 position)
    {
        if (Entity == null)
            return false;

        var vec = Entity.Position;
        Entity.Position = position;

        var result = Colliding(other);
        
        Entity.Position = vec;
        return result;
    }

    /// <summary>
    ///     Check collision with all colliders that have the given tags.
    /// </summary>
    /// <param name="tags">The tags to check.</param>
    public bool Colliding(Tag tags)
    {
        foreach (var collider in Colliders)
        {
            if (collider.Tag.ContainsAny(tags) && collider.Collideable && collider != this && collider.Colliding(this))
                return true;
        }
        return false;
    }

    /// <summary>
    ///     Check collision with all colliders that have the given tags at the specified position.
    /// </summary>
    /// <param name="tags">The tags to check.</param>
    /// <param name="position">The position to check.</param>
    public bool Colliding(Tag tags, Vector2 position)
    {
        if (Entity == null)
            return false;

        var vec = Entity.Position;
        Entity.Position = position;

        var result = Colliding(tags);
        
        Entity.Position = vec;
        return result;
    }

    /// <summary>
    ///     Check collision with a Box Collider.
    /// </summary>
    /// <param name="collider">The collider to check.</param>
    public abstract bool Colliding(BoxCollider collider);
}