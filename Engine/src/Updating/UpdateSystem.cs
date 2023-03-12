using Battery.Framework;

namespace Battery.Engine;

/// <summary>
///     Provides the component updating logic.
/// </summary>
public class UpdateSystem : ComponentSystem
{
    /// <summary>
    ///     The table that store all the updaters.
    /// </summary>
    public Table<IUpdater> Updaters = new Table<IUpdater>();

    // The delay to realize the next update.
    private float _delay;

    /// <summary>
    ///     Creates a new Update System.
    /// </summary>
    public UpdateSystem()
        : base(true, false)
    {
    }

    /// <inheritdoc/>
    public override void Begin()
    {
        foreach (var updater in Updaters)
        {
            if (Tag.ContainsAny(updater.Tag))
                updater.Begin();
        }
    }

    /// <inheritdoc/>
    public override void End()
    {
        foreach (var updater in Updaters)
        {
            if (Tag.ContainsAny(updater.Tag))
                updater.End();
        }
    }

    /// <inheritdoc/>
    public override void Update()
    {
        if (_delay > 0)
        {
            _delay -= Time.Delta;
            return;
        }

        foreach (var updater in Updaters)
        {
            if (updater.Active && Tag.ContainsAny(updater.Tag))
                updater.Update();
        }
    }

    /// <summary>
    ///     Add a Updatable Component to the System.
    /// </summary>
    /// <param name="component">The component to add.</param>
    public override void Add(Component component)
    {
        if (component is IUpdater updater)
        {
            if (Game.Running)
                updater.Begin();

            Updaters.Add(updater);
        }
    }

    /// <summary>
    ///     Remove a Updatable Component from the System.
    /// </summary>
    /// <param name="component">The component to remove.</param>
    public override void Remove(Component component)
    {
        if (component is IUpdater updater)
        {            
            if (Game.Running)
                updater.End();

            Updaters.Remove(updater);
        }
    }

    /// <summary>
    ///     Sets the delay time to the next update.
    /// </summary>
    /// <param name="num">The new delay value.</param>
    public void SetDelay(float num)
    {
        _delay = num;
    }

    /// <summary>
    ///     Gets the delay time to the next update.
    /// </summary>
    public float GetDelay()
    {
        return Math.Clamp(_delay, 0, _delay);
    }
}