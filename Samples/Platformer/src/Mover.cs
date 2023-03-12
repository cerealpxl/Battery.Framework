using System.Numerics;
using Battery.Framework;
using Battery.Engine;

namespace Platformer;

/// <summary>
///     Component that handle the physics behavior.
/// </summary>
public class Mover : Updater 
{
    /// <summary>
    ///     The Tag that represents the Actors.
    /// </summary>
    public static Tag Actor = new Tag(2u);

    /// <summary>
    ///     The Tag that represents the Solids.
    /// </summary>
    public static Tag Solid = new Tag(4u);

    /// <summary>
    ///     The actual speed of the mover.
    /// </summary>
    public Vector2 Speed;

    /// <summary>
    ///     The target speed of the mover.
    /// </summary>
    public Vector2 TargetSpeed;

    /// <summary>
    ///     The amount used to approximate the actual speed to the target speed.
    /// </summary>
    public Vector2 Acceleration;

    /// <summary>
    ///     The total amount to move.
    /// </summary>
    public Vector2 Counter;

    /// <summary>
    ///     The collider used by the Mover.
    /// </summary>
    public Collider Collider;

    /// <summary>
    ///     Creates a new instance of <see cref="Mover"/> component.
    /// </summary>
    /// <param name="collider">The collider used by the Mover.</param>
    public Mover(Collider collider)
    {
        Collider = collider;
    }

    /// <summary>
    ///     Moves the entity by the actual speed and approaches the speed to the target.
    /// </summary>
    public override void Update()
    {
        if (Entity == null)
            return;

        Counter += Speed;

        var move = Counter.Round();
        var sign = new Vector2(move.X.Sign(), move.Y.Sign());

        Counter -= move;

        MoveExactH(move.X);
        MoveExactV(move.Y);

        Speed = Speed.Approach(TargetSpeed, Acceleration);
    }

    /// <summary>
    ///     Moves horizontally by using the given amount of pixels.
    /// </summary>
    /// <param name="move">The amount of pixels to move.</param>
    public void MoveExactH(float move)
    {
        if (Entity == null)
            return;

        var sign = move.Sign();

        while (move != 0)
        {
            if (Collider.Colliding(Solid, Entity.Position + Vector2.UnitX * sign))
                break;

            move            -= sign;
            Entity.Position += Vector2.UnitX * sign;
        }
    }

    /// <summary>
    ///     Moves vertically by using the given amount of pixels.
    /// </summary>
    /// <param name="move">The amount of pixels to move.</param>
    public void MoveExactV(float move)
    {
        if (Entity == null)
            return;

        var sign = move.Sign();

        while (move != 0)
        {
            if (Collider.Colliding(Solid, Entity.Position + Vector2.UnitY * sign))
                break;

            move            -= sign;
            Entity.Position += Vector2.UnitY * sign;
        }
    }
}