using Anchorpoint.Core.Units;

namespace Anchorpoint.App.Events
{
    /// <summary>
    /// A timed world event that spawns and removes its own objects.
    /// </summary>
    public interface IGameEvent
    {
        /// <summary>
        /// True if the event ended successfully.
        /// </summary>
        bool WasSuccess { get; }

        /// <summary>
        /// Extra text added to the start message, or null.
        /// </summary>
        string SpawnDescription { get; }

        /// <summary>
        /// Spawns the event's objects.
        /// </summary>
        void Begin();

        /// <summary>
        /// Updates the event; returns false when it's done.
        /// </summary>
        bool Tick(float dt);

        /// <summary>
        /// Removes everything the event spawned.
        /// </summary>
        void Cleanup();
    }
}
