using System.Numerics;
using Battery.Framework;
using Battery.Engine;

namespace Platformer;

/// <summary>
///     Provides the Player behavior.
/// </summary>
public class Player : Updater
{
    /// <summary>
    ///     The Collider used by the Player.
    /// </summary>
    public BoxCollider Collider = new BoxCollider(Tag.Default, new Rectangle(0, 0, 12, 16));

    /// <summary>
    ///     The Mover used by the Player.
    /// </summary>
    public Mover Mover;

    /// <summary>
    ///     Whether the Player is jumping.
    /// </summary>
    public bool Jumping { get; private set; }

    /// <summary>
    ///     Creates a new instance of <see cref="Player"/> component.
    /// </summary>
    public Player()
    {
        Mover = new Mover(Collider);
        OnAdd += () => 
        {
            Entity?.Add(Collider);
            Entity?.Add(new ColliderRenderer(Collider, Color.Green));
            Entity?.Add(Mover);
        };
    }

    /// <summary>
    ///     Updates the Player.
    /// </summary>
    public override void Update()
    {
        if (Entity == null)
            return;

        var axis     = Keyboard.Down(KeyConstant.Left) ? -1 : (Keyboard.Down(KeyConstant.Right) ? 1 : 0);
        var grounded = Collider.Colliding(Mover.Solid, Entity.Position + Vector2.UnitY);

        // Assign the speed variables.
        Mover.TargetSpeed.X  = 1.5f * axis;
        Mover.Acceleration.X = 0.25f;

        // Fall!
        if (!grounded)
        {
            Jumping = Jumping && Mover.Speed.Y < 0 && Keyboard.Down(KeyConstant.C);
            Mover.TargetSpeed.Y  = 3.5f;
            Mover.Acceleration.Y = 0.3f * (Jumping ? 0.45f : 1.0f);
        }

        // Jump!
        if (Keyboard.Pressed(KeyConstant.C) && grounded)
        {
            Mover.Speed.X += 0.35f * axis;
            Mover.Speed.Y  = -3.2f;
            Jumping = true;
        }
    }
}