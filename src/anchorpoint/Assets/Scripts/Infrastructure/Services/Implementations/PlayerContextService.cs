using Anchorpoint.Core.Events;
using Anchorpoint.Core.Units;
using Anchorpoint.Infrastructure.Services.Interfaces;
using UnityEngine;

namespace Anchorpoint.Infrastructure.Services.Implementations
{
    /// <summary>
    /// Tracks the player unit and current area.
    /// </summary>
    public sealed class PlayerContextService : IPlayerContextService
    {
        /// <inheritdoc/>
        public Unit PlayerUnit { get; private set; }

        /// <inheritdoc/>
        public Transform Player => PlayerUnit?.Root;

        /// <inheritdoc/>
        public EventContext Current { get; private set; }

        /// <inheritdoc/>
        public void SetPlayer(Unit player)
        {
            PlayerUnit = player;
        }

        /// <inheritdoc/>
        public void SetOnBase(bool onBase)
        {
            Current = onBase ? EventContext.OnBase : EventContext.Exploring;
        }
    }
}
