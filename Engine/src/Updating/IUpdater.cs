using Battery.Framework;

namespace Battery.Engine;

/// <summary>
///     Represents a Updater Component.
///     Provides the Update methods.
/// </summary>
public interface IUpdater : ITagged
{
    /// <summary>
    ///     Whether the component is visible.
    /// </summary>
    bool Active { get; set; }

    /// <summary>
    ///     Called when the Scene begins.
    /// </summary>
    abstract void Begin();

    /// <summary>
    ///     Called when the Scene ends.
    /// </summary>
    abstract void End();

    /// <summary>
    ///     Called to update the component.
    /// </summary>
    abstract void Update();
}

/// <summary>
///     IUpdater method extensions.
/// </summary>
public static class IUpdaterExt
{
    /// <summary>
    ///     Activate this updater.
    /// </summary>
    public static IUpdater Activate(this IUpdater updater)
    {
        updater.Active = true;
        return updater;
    }

    /// <summary>
    ///     Deactivate this updater.
    /// </summary>
    public static IUpdater Deactivate(this IUpdater updater)
    {
        updater.Active = false;
        return updater;
    }
}