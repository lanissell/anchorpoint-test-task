using Anchorpoint.Core.Events;
using Anchorpoint.Infrastructure.Environment;
using Anchorpoint.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Anchorpoint.Infrastructure.Services.Implementations
{
    /// <summary>
    /// Tracks spawn points and picks ones near the player by area.
    /// </summary>
    public sealed class SpawnPointService : ISpawnPointService
    {
        private readonly IPlayerContextService playerContext;
        private readonly List<SpawnPoint> points = new List<SpawnPoint>();

        /// <summary>
        /// Creates the service with the player context.
        /// </summary>
        public SpawnPointService(IPlayerContextService playerContext)
        {
            this.playerContext = playerContext;
        }

        /// <inheritdoc/>
        public void Register(SpawnPoint point)
        {
            if (point != null && !points.Contains(point))
            {
                points.Add(point);
            }
        }

        /// <inheritdoc/>
        public void Unregister(SpawnPoint point)
        {
            points.Remove(point);
        }

        /// <inheritdoc/>
        public IReadOnlyList<Vector3> GetPoints(EventContext area, int count, float maxSpawnDistance)
        {
            var player = playerContext.Player;

            if (player == null)
            {
                return Array.Empty<Vector3>();
            }

            var playerPos = player.position;
            bool unlimited = maxSpawnDistance <= 0f;

            return points
                .Where(p =>
                    p.Area == area &&
                    (unlimited || Vector3.Distance(p.Position, playerPos) <= maxSpawnDistance))
                .OrderBy(_ => Guid.NewGuid())
                .Take(count)
                .Select(p => p.Position)
                .ToList();
        }
    }
}
