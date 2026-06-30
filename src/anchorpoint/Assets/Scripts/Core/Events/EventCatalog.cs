using Anchorpoint.Core.Events.Base;
using System.Collections.Generic;
using UnityEngine;

namespace Anchorpoint.App.Events
{
    /// <summary>
    /// Asset listing events and how often they fire.
    /// </summary>
    [CreateAssetMenu(fileName = "EventCatalog", menuName = "Anchorpoint/Event Catalog")]
    public sealed class EventCatalog : ScriptableObject
    {
        /// <summary>
        /// Shortest gap between events.
        /// </summary>
        [field: SerializeField]
        public float MinInterval { get; private set; } = 5f;

        /// <summary>
        /// Longest gap between events.
        /// </summary>
        [field: SerializeField]
        public float MaxInterval { get; private set; } = 15f;

        /// <summary>
        /// How long a successful event lingers before cleanup.
        /// </summary>
        [field: SerializeField]
        public float SuccessCleanupDelay { get; private set; } = 2f;

        [SerializeReference, SubclassSelector]
        private List<GameEventConfigBase> events = new List<GameEventConfigBase>();

        /// <summary>
        /// All events in this catalog.
        /// </summary>
        public IReadOnlyList<GameEventConfigBase> Events => events;
    }
}
