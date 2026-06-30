using System;
using UnityEngine;

namespace Anchorpoint.Infrastructure.Services.PoolEntries
{
    /// <summary>
    /// A pooled decal that returns itself after a set time.
    /// </summary>
    public sealed class DecalEntry : MonoBehaviour
    {
        [SerializeField]
        private float lifetime = 10f;

        private Action onReleased;

        /// <summary>
        /// Sets the release callback.
        /// </summary>
        public void Initialize(Action onReleased)
        {
            this.onReleased = onReleased;
        }

        /// <summary>
        /// Starts the countdown to return the decal to the pool.
        /// </summary>
        public void Play()
        {
            CancelInvoke();
            Invoke(nameof(Release), lifetime);
        }

        private void Release()
        {
            onReleased?.Invoke();
        }
    }
}
