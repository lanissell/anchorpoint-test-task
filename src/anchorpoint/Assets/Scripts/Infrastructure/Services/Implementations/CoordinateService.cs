using Anchorpoint.Infrastructure.Services.Interfaces;
using UnityEngine;

namespace Anchorpoint.Infrastructure.Services.Implementations
{
    /// <summary>
    /// Turns world positions into 2D map coordinates.
    /// </summary>
    public sealed class CoordinateService : ICoordinateService
    {
        private readonly IPlayerContextService playerContext;

        /// <summary>
        /// Creates the service with the player context.
        /// </summary>
        public CoordinateService(IPlayerContextService playerContext)
        {
            this.playerContext = playerContext;
        }

        /// <inheritdoc/>
        public Vector2 PlayerCoordinates => playerContext.Player != null
            ? WorldToMap(playerContext.Player.position)
            : Vector2.zero;

        /// <inheritdoc/>
        public Vector2 WorldToMap(Vector3 worldPosition)
            => new Vector2(worldPosition.x, worldPosition.z);
    }
}
