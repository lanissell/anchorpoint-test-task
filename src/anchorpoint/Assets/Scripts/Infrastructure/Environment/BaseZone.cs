using Anchorpoint.Infrastructure.Services.Interfaces;
using UnityEngine;

namespace Anchorpoint.Infrastructure.Environment
{
    /// <summary>
    /// Marks the player as on base while inside this trigger.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public sealed class BaseZone : MonoBehaviour
    {
        private IPlayerContextService playerContext;

        /// <summary>
        /// Sets up the zone with the player context.
        /// </summary>
        public void Initialze(IPlayerContextService playerContextService)
        {
            playerContext = playerContextService;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform == playerContext.Player)
            {
                playerContext.SetOnBase(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (playerContext.Player != null && other.transform == playerContext.Player)
            {
                playerContext.SetOnBase(false);
            }
        }
    }
}
