using UnityEngine;

namespace Anchorpoint.Infrastructure.Services.Interfaces
{
    /// <summary>
    /// Turns world positions into 2D map coordinates.
    /// </summary>
    public interface ICoordinateService
    {
        /// <summary>
        /// The player's map position.
        /// </summary>
        Vector2 PlayerCoordinates { get; }

        /// <summary>
        /// Maps a world position to 2D, dropping height.
        /// </summary>
        Vector2 WorldToMap(Vector3 worldPosition);
    }
}
