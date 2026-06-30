using Anchorpoint.App.Events;
using Anchorpoint.App.Units.Brains;
using Anchorpoint.App.Units.UnitComponents;
using Anchorpoint.Core.Events;
using Anchorpoint.Core.Units;
using Anchorpoint.Infrastructure.Services;
using Anchorpoint.Infrastructure.Services.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Anchorpoint.App.GameEvents.Events
{
    /// <summary>
    /// Spawns a wave of enemies for a set time.
    /// </summary>
    public sealed class EnemyAttackEvent : IGameEvent
    {
        /// <inheritdoc/>
        public bool WasSuccess => wasSuccess;

        /// <inheritdoc/>
        public string SpawnDescription => null;

        private readonly UnitInstaller npcPrefab;
        private readonly int count;
        private readonly float lifetime;
        private readonly EventContext area;

        private readonly float maxSpawnDistance;

        private readonly ISpawnPointService spawnPointService;
        private readonly IPoolService poolService;
        private readonly IUnitFactoryService factoryService;
        private readonly IUnitTickService tickService;

        private float remaining;
        private bool wasSuccess;
        private readonly List<(Unit unit, UnitInstaller installer)> spawned = new List<(Unit, UnitInstaller)>();

        /// <summary>
        /// Creates the event with prefab, count, lifetime, and area.
        /// </summary>
        public EnemyAttackEvent(UnitInstaller npcPrefab, int count, float lifetime, EventContext area, float maxSpawnDistance)
        {
            this.npcPrefab = npcPrefab;
            this.count = count;
            this.lifetime = lifetime;
            this.area = area;
            this.maxSpawnDistance = maxSpawnDistance;

            spawnPointService = AppLoader.ServiceResolver.Resolve<ISpawnPointService>();
            poolService = AppLoader.ServiceResolver.Resolve<IPoolService>();
            factoryService = AppLoader.ServiceResolver.Resolve<IUnitFactoryService>();
            tickService = AppLoader.ServiceResolver.Resolve<IUnitTickService>();
        }

        /// <inheritdoc/>
        public void Begin()
        {
            remaining = lifetime;
            wasSuccess = false;

            var found = spawnPointService.GetPoints(area, count, maxSpawnDistance);

            for (int i = 0; i < found.Count; i++)
            {
                Spawn(found[i]);
            }
        }

        private bool AllDead()
        {
            foreach (var (unit, _) in spawned)
            {
                if (!unit.TryGet(out HealthComponent health) || !health.IsDead)
                {
                    return false;
                }
            }

            return true;
        }

        /// <inheritdoc/>
        public bool Tick(float dt)
        {
            // Nothing spawned (no matching points): resolve now so the event is cleaned up.
            if (spawned.Count == 0 || AllDead())
            {
                wasSuccess = true;
                return false;
            }

            remaining -= dt;

            if (remaining <= 0f)
            {
                wasSuccess = false;
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public void Cleanup()
        {
            foreach (var (unit, installer) in spawned)
            {
                unit.Reset();
                tickService.Unregister(unit);
                unit.Dispose();
                poolService.Release(npcPrefab, installer);
            }

            spawned.Clear();
        }

        private void Spawn(Vector3 point)
        {
            var installer = poolService.Get(npcPrefab, point, Quaternion.identity);
            var unit = installer.Initialize(factoryService);

            spawned.Add((unit, installer));
        }
    }
}
