using System.Numerics;
using Battery.Framework;

namespace Battery.Engine;

/// <summary>
///     A Box Collider.
/// </summary>
public class BoxCollider : Collider
{
    /// <summary>
    ///     The boundaries of the Collider.
    /// </summary>
    public Rectangle Bounds;

    /// <summary>
    ///     The boundaries of the Collider relative to the Entity Position.
    /// </summary>
    public Rectangle WorldBounds => Bounds + (Entity == null ? Vector2.Zero : Entity.Position);

    /// <summary>
    ///     Creates a new instance of <see cref="BoxCollider"/> component.
    /// </summary>
    /// <param name="tags">The tags of the Collider.</param>
    /// <param name="bounds">The boundaries of the Collider.</param>
    public BoxCollider(Tag tags, Rectangle bounds)
        : base(tags)
    {
        Bounds = bounds;
    }

    /// <inheritdoc/>
    public override bool Colliding(BoxCollider collider)
    {
        return WorldBounds.Intersects(collider.WorldBounds);
    }
}