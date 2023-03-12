using Battery.Framework;

namespace Battery.Engine;

/// <summary>
///     Provides the logic of the ECS Scene handling.
/// </summary>
public class SceneManager : GameManager
{
    /// <summary>
    ///     The current scene of the <see cref="GameManager" />.
    /// </summary>
    public Scene Current;

    /// <summary>
    ///     Action called when the the code of a <see cref="Scene" /> crashes.
    /// </summary>
    public Action<Scene, Exception>? OnCrash;

    /// <summary>
    ///     Called before the Update events.
    /// </summary>
    public Action? OnUpdateBegin;

    /// <summary>
    ///     Called after the Update events.
    /// </summary>
    public Action? OnUpdateEnd;

    // The next scene.
    private Scene? _next;

    /// <summary>
    ///     Creates a new instance of the <see cref="SceneManager" /> class.
    /// </summary>
    /// <param name="scene">The optional first <see cref="Scene" />.</param>
    public SceneManager(Scene? scene = null)
        => Current = scene ?? new Scene();

    /// <summary>
    ///     Creates a new instance of the <see cref="SceneManager" /> class.
    /// </summary>
    public SceneManager()
        : this(null)
    {
    }

    /// <summary>
    ///     Begins the current Scene.
    /// </summary>
    public override void Begin()
    {
        if (OnCrash != null)
        {
            try
            {
                Current.Begin();
            }
            catch (Exception e)
            {
                OnCrash(Current, e);
            }
        }
        else
            Current.Begin();
    }
    
    /// <summary>
    ///     Ends the current Scene.
    /// </summary>
    public override void End()
    {
        if (OnCrash != null)
        {
            try
            {
                Current.End();
            }
            catch (Exception e)
            {
                OnCrash(Current, e);
            }
        }
        else
            Current.End();
    }

    /// <summary>
    ///     Updates the current Scene.
    /// </summary>
    public override void Update()
    {
        // Swap the current scene.
        if (_next != null)
        {
            Current.End();
            Current = _next;
            Current.Begin();
        }

        OnUpdateBegin?.Invoke();

        if (OnCrash != null)
        {
            try
            {
                Current.Update();
            }
            catch (Exception e)
            {
                OnCrash(Current, e);
            }
        }
        else
            Current.Update();

        OnUpdateEnd?.Invoke();
    }

    /// <inheritdoc/>
    public override void RenderBegin()
    {
        if (OnCrash != null)
        {
            try
            {
                Current.RenderBegin();
            }
            catch (Exception e)
            {
                OnCrash(Current, e);
            }
        }
        else
            Current.RenderBegin();
    }

    /// <summary>
    ///     Renders the current Scene.
    /// </summary>
    public override void Render()
    {
        if (OnCrash != null)
        {
            try
            {
                Current.Render();
            }
            catch (Exception e)
            {
                OnCrash(Current, e);
            }
        }
        else
            Current.Render();
    }

    /// <inheritdoc/>
    public override void RenderEnd()
    {
        if (OnCrash != null)
        {
            try
            {
                Current.RenderEnd();
            }
            catch (Exception e)
            {
                OnCrash(Current, e);
            }
        }
        else
            Current.RenderEnd();
    }

    /// <summary>
    ///     Swap the current scene in the next update.
    /// </summary>
    /// <param name="scene">The scene to set.</param>
    public void Swap(Scene scene)
        => _next = scene;
}