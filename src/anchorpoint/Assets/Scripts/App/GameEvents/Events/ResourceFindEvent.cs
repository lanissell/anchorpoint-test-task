using Anchorpoint.App.Events;
using Anchorpoint.Core.Events;
using Anchorpoint.Core.Interaction;
using Anchorpoint.Infrastructure;
using Anchorpoint.Infrastructure.Services.Interfaces;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Anchorpoint.App.GameEvents.Events
{
    /// <summary>
    /// Spawns a pickup to collect before it times out.
    /// </summary>
    public sealed class ResourceFindEvent : IGameEvent
    {
        /// <inheritdoc/>
        public bool WasSuccess => wasSuccess;

        /// <inheritdoc/>
        public string SpawnDescription => spawnDescription;

        private readonly IReadOnlyList<Pickup> prefabs;
        private readonly float timeout;
        private readonly EventContext area;
        private readonly float maxSpawnDistance;

        private readonly ISpawnPointService spawnPointService;
        private readonly IPoolService poolService;
        private readonly ICoordinateService coordinateService;

        private float remaining;
        private Pickup prefab;
        private Pickup instance;
        private bool collected;
        private bool wasSuccess;
        private string spawnDescription;

        /// <summary>
        /// Creates the event with pickups, timeout, and area.
        /// </summary>
        public ResourceFindEvent(IReadOnlyList<Pickup> prefabs, float timeout, EventContext area, float maxSpawnDistance)
        {
            this.prefabs = prefabs;
            this.timeout = timeout;
            this.area = area;
            this.maxSpawnDistance = maxSpawnDistance;

            spawnPointService = AppLoader.ServiceResolver.Resolve<ISpawnPointService>();
            poolService = AppLoader.ServiceResolver.Resolve<IPoolService>();
            coordinateService = AppLoader.ServiceResolver.Resolve<ICoordinateService>();
        }

        /// <inheritdoc/>
        public void Begin()
        {
            remaining = timeout;
            collected = false;
            wasSuccess = false;
            spawnDescription = null;

            if (prefabs == null || prefabs.Count == 0)
            {
                return;
            }

            var found = spawnPointService.GetPoints(area, 1, maxSpawnDistance);

            if (found.Count == 0)
            {
                return;
            }

            var point = found[0];

            var coords = coordinateService.WorldToMap(point);
            spawnDescription = $"({coords.x:F0}, {coords.y:F0})";

            prefab = prefabs[Random.Range(0, prefabs.Count)];
            instance = poolService.Get(prefab, point, Quaternion.identity);
            instance.Initialize(() =>
            {
                collected = true;
                poolService.Release(prefab, instance);
            });
        }

        /// <inheritdoc/>
        public bool Tick(float dt)
        {
            if (collected)
            {
                wasSuccess = true;
                return false;
            }

            if (timeout > 0f)
            {
                remaining -= dt;

                if (remaining <= 0f)
                {
                    return false;
                }
            }

            return true;
        }

        /// <inheritdoc/>
        public void Cleanup()
        {
            if (!collected && instance != null)
            {
                instance.Initialize(null);
                poolService.Release(prefab, instance);
            }

            instance = null;
            prefab = null;
            collected = false;
            spawnDescription = null;
        }
    }
}
