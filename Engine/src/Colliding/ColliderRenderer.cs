using Battery.Framework;

namespace Battery.Engine;

/// <summary>
///     Component that render the given <see cref="Engine.Collider"/>.
/// </summary>
public class ColliderRenderer : Renderer
{
    /// <summary>
    ///     The collider to render.
    /// </summary>
    public Collider Collider;

    /// <summary>
    ///     The color to render the shape.
    /// </summary>
    public Color Color;

    /// <summary>
    ///     Creates a new instance of <see cref="ColliderRenderer"/> component.
    /// </summary>
    /// <param name="collider">The collider to render.</param>
    /// <param name="color">The color used to render the collider.</param>
    public ColliderRenderer(Collider collider, Color? color = null)
    {
        Collider = collider;
        Color    = color ?? Color.Red;
    }

    /// <summary>
    ///     Renders the collider.
    /// </summary>
    public override void Render()
    {
        if (Collider is BoxCollider box)
        {
            Batch?.HollowRectangle(box.WorldBounds, Color);
        }
    }
}