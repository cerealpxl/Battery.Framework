using Battery.Framework;

namespace Battery.Engine;

/// <summary>
///     Class that extends the updater components.
/// </summary>
public class Updater : Component, IUpdater
{
    /// <inheritdoc/>
    public bool Active { get; set; } = true;

    /// <inheritdoc/>
    public Tag Tag { get; set; } = Tag.Default;

    /// <inheritdoc/>
    public virtual void Begin()
    {
    }

    /// <inheritdoc/>
    public virtual void End()
    {
    }

    /// <inheritdoc/>
    public virtual void Update()
    {
    }
}