using System;
using UnityEngine;

namespace Anchorpoint.Infrastructure.Services.PoolEntries
{
    /// <summary>
    /// A pooled particle that returns itself when it stops.
    /// </summary>
    [RequireComponent(typeof(ParticleSystem))]
    public sealed class ParticleEntry : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem particle;

        private Action onStopped;

        /// <summary>
        /// Sets the release callback.
        /// </summary>
        public void Initialize(Action onStopped)
        {
            this.onStopped = onStopped;

            var main = particle.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }

        /// <summary>
        /// Plays the particle.
        /// </summary>
        public void Play()
        {
            particle.Play();
        }

        private void OnParticleSystemStopped()
        {
            onStopped?.Invoke();
        }
    }
}
