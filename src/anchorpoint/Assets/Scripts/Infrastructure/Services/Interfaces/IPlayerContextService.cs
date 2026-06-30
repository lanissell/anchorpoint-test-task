using Anchorpoint.Core.Events;
using Anchorpoint.Core.Units;
using UnityEngine;

namespace Anchorpoint.Infrastructure.Services.Interfaces
{
    /// <summary>
    /// Tracks the player unit and current area.
    /// </summary>
    public interface IPlayerContextService
    {
        /// <summary>
        /// The player unit.
        /// </summary>
        Unit PlayerUnit { get; }

        /// <summary>
        /// The player's transform.
        /// </summary>
        Transform Player { get; }

        /// <summary>
        /// The area the player is in.
        /// </summary>
        EventContext Current { get; }

        /// <summary>
        /// Sets the player unit.
        /// </summary>
        void SetPlayer(Unit player);

        /// <summary>
        /// Sets whether the player is on base.
        /// </summary>
        void SetOnBase(bool onBase);
    }
}
