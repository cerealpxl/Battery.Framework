using System.Numerics;
using Battery.Framework;

namespace Battery.Engine;

/// <summary>
///     Provides the component rendering logic.
/// </summary>
public class RenderSystem : ComponentSystem
{
    /// <summary>
    ///     The table that stores all the renderers of the system.
    /// </summary>
    public Table<IRenderer> Renderers = new Table<IRenderer>();

    /// <summary>
    ///     The table that stores all the cameras of the system.
    /// </summary>
    public Table<Camera> Cameras = new Table<Camera>();

    /// <summary>
    ///     The last camera rendered by the System.
    /// </summary>
    public Camera? CurrentCamera;

    /// <summary>
    ///     Creates a new Render System.
    /// </summary>
    public RenderSystem()
        : base(false, true)
    {
    }

    /// <inheritdoc />
    public override void Begin()
    {
        if (Cameras.Count == 0 && Game.Instance != null)
        {
            Cameras.Add(new Camera(
                Game.Instance.Graphics, 
                Game.Instance.Platform.Width,
                Game.Instance.Platform.Height
            ));
        }
        
        base.Begin();
    }

    /// <inheritdoc />
    public override void RenderBegin()
    {
        if (Game.Instance is Game)
            Game.Instance.Graphics.Batch?.Clear();
    }

    /// <summary>
    ///     Renders all the renderable components to the enabled cameras.
    /// </summary>
    public override void Render()
    {
        if (!(Game.Instance is Game))
            return;

        // Render all the game cameras.
        var graphics = Game.Instance.Graphics;

        // Loop and draw the renderers to every camera in the system.
        foreach (var camera in Cameras)
        {
            if (!camera.Enabled)
                continue;

            // Sets the camera surface then draw the renderers.
            graphics.Clear(camera.Surface, camera.ClearColor);
            graphics.Batch?.PushTarget(camera.Surface, camera.Matrix);
            CurrentCamera = camera;

            foreach (var renderer in Renderers)
            {
                if (renderer.Visible && Tag.ContainsAny(renderer.Tag))
                    renderer.RenderBegin();
            }

            foreach (var renderer in Renderers)
            {
                if (renderer.Visible && Tag.ContainsAny(renderer.Tag))
                    renderer.Render();
            }

            foreach (var renderer in Renderers)
            {
                if (renderer.Visible && Tag.ContainsAny(renderer.Tag))
                    renderer.RenderEnd();
            }

            graphics.Batch?.PopTarget();
        }

        // Render all the game cameras.
        foreach (var camera in Cameras)
        {
            if (camera.Visible)
                graphics.Batch?.Texture(camera.Surface, Vector2.Zero);
        }
    }

    /// <inheritdoc />
    public override void RenderEnd()
    {
        if (Game.Instance is Game)
            Game.Instance.Graphics.Batch?.Present();
    }

    /// <inheritdoc/>
    public override void Add(Component component)
    {
        if (component is IRenderer renderer)
            Renderers.Add(renderer);
    }

    /// <inheritdoc/>
    public override void Remove(Component component)
    {
        if (component is IRenderer renderer)
            Renderers.Remove(renderer);
    }
}