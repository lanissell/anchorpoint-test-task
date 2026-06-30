using Anchorpoint.App.Events;

namespace Anchorpoint.Infrastructure.Services.Interfaces
{
    /// <summary>
    /// Runs random world events from a catalog.
    /// </summary>
    public interface IEventService
    {
        /// <summary>
        /// Starts running events from the catalog.
        /// </summary>
        void Begin(EventCatalog catalog);

        /// <summary>
        /// Updates running events and fires new ones.
        /// </summary>
        void Tick(float dt);
    }
}
