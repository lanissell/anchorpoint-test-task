using Anchorpoint.Core.Events;
using Anchorpoint.Infrastructure.Environment;
using System.Collections.Generic;
using UnityEngine;

namespace Anchorpoint.Infrastructure.Services.Interfaces
{
    /// <summary>
    /// Tracks spawn points and picks ones out of the player's sight.
    /// </summary>
    public interface ISpawnPointService
    {
        /// <summary>
        /// Adds a spawn point.
        /// </summary>
        void Register(SpawnPoint point);

        /// <summary>
        /// Removes a spawn point.
        /// </summary>
        void Unregister(SpawnPoint point);

        IReadOnlyList<Vector3> GetPoints(EventContext area, int count, float maxSpawnDistance);
    }
}
