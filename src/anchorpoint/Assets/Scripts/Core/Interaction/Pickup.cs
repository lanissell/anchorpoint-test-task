using System;
using UnityEngine;

namespace Anchorpoint.Core.Interaction
{
    /// <summary>
    /// Something the player can pick up.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public sealed class Pickup : MonoBehaviour, IPickup
    {
        private Action onCollect;

        /// <summary>
        /// Sets a release callback; without one Collect destroys the object.
        /// </summary>
        public void Initialize(Action onCollect)
        {
            this.onCollect = onCollect;
        }

        /// <inheritdoc/>
        public void Collect()
        {
            if (onCollect != null)
            {
                var cb = onCollect;
                onCollect = null;
                cb.Invoke();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
