using Anchorpoint.App.Events;
using System;
using UnityEngine;

namespace Anchorpoint.Core.Events.Base
{
    /// <summary>
    /// Serialized settings that build a runtime event.
    /// </summary>
    [Serializable]
    public abstract class GameEventConfigBase
    {
        /// <summary>
        /// Where this event can fire.
        /// </summary>
        [field: Header("Context"), SerializeField]
        public EventContext Context { get; private set; }

        /// <summary>
        /// Message shown when the event starts.
        /// </summary>
        [field: Header("Message"), SerializeField]
        public string StartMessage { get; private set; }

        /// <summary>
        /// Message shown when the event succeeds.
        /// </summary>
        [field: SerializeField]
        public string SuccessMessage { get; private set; }

        /// <summary>
        /// Message shown when the event fails.
        /// </summary>
        [field: SerializeField]
        public string FailureMessage { get; private set; }

        /// <summary>
        /// Builds the runtime event.
        /// </summary>
        public abstract IGameEvent Build();
    }
}
