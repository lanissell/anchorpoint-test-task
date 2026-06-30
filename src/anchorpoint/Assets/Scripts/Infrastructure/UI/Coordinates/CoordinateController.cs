using Anchorpoint.Infrastructure.Services.Interfaces;

namespace Anchorpoint.Infrastructure.UI.Coordinates
{
    /// <summary>
    /// Updates the coordinate view each tick.
    /// </summary>
    public sealed class CoordinateController
    {
        private readonly ICoordinateService coordinateService;
        private readonly CoordinateView view;

        /// <summary>
        /// Creates the controller with the coordinate service and view.
        /// </summary>
        public CoordinateController(ICoordinateService coordinateService, CoordinateView view)
        {
            this.coordinateService = coordinateService;
            this.view = view;
        }

        /// <summary>
        /// Refreshes the view with the player's coordinates.
        /// </summary>
        public void Tick(float dt)
        {
            view.Render(coordinateService.PlayerCoordinates);
        }
    }
}
