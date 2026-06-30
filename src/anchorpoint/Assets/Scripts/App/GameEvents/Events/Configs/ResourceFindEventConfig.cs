using Anchorpoint.App.Events;
using Anchorpoint.Core.Events.Base;
using Anchorpoint.Core.Interaction;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Anchorpoint.App.GameEvents.Events.Configs
{
    /// <summary>
    /// Builds a resource-find event.
    /// </summary>
    [Serializable]
    public sealed class ResourceFindEventConfig : GameEventConfigBase
    {
        [Header("Spawn")]
        [SerializeField]
        private List<Pickup> prefabs;

        [SerializeField]
        private float timeout = 30f;

        [SerializeField]
        private float maxSpawnDistance;

        /// <inheritdoc/>
        public override IGameEvent Build() => new ResourceFindEvent(prefabs, timeout, Context, maxSpawnDistance);
    }
}
