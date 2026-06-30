using Anchorpoint.App.Events;
using Anchorpoint.App.Units.Brains;
using Anchorpoint.Core.Events.Base;
using Anchorpoint.Infrastructure.Services;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Anchorpoint.App.GameEvents.Events.Configs
{
    /// <summary>
    /// Builds an enemy wave event.
    /// </summary>
    [Serializable]
    public sealed class EnemyAttackEventConfig : GameEventConfigBase
    {
        [Header("Spawn")]
        [SerializeField]
        private UnitInstaller npcPrefab;

        [SerializeField]
        private int minCount = 2;

        [SerializeField]
        private int maxCount = 3;

        [SerializeField]
        private float lifetime = 20f;

        [SerializeField]
        private float maxSpawnDistance;

        /// <inheritdoc/>
        public override IGameEvent Build() => new EnemyAttackEvent(
            npcPrefab,
            Random.Range(minCount, maxCount + 1),
            lifetime,
            Context,
            maxSpawnDistance);
    }
}
