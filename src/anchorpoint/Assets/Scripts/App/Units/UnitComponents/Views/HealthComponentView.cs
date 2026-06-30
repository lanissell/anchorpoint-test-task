using Anchorpoint.Core.Units;
using Anchorpoint.Core.Units.Base;
using Anchorpoint.Infrastructure.Services;
using Anchorpoint.Infrastructure.Services.Interfaces;
using Anchorpoint.Infrastructure.Services.PoolEntries;
using UnityEngine;
using UnityEngine.Events;

namespace Anchorpoint.App.Units.UnitComponents.Views
{
    /// <summary>
    /// Plays hit and death feedback.
    /// </summary>
    public sealed class HealthComponentView : MonoBehaviour, IUnitComponent
    {
        [SerializeField]
        private UnityEvent OnDead;

        [Header("Animation")]
        [SerializeField]
        private string deathTrigger = "Dead";

        [SerializeField]
        private Animator animator;

        [Header("Sound")]
        [SerializeField]
        private SoundEntry soundPrefab;

        [SerializeField]
        private SoundEffect deathSound;

        [SerializeField]
        private SoundEffect hitSound;

        private Unit unit;
        private IPoolService poolService;

        /// <inheritdoc/>
        public void Attach(Unit unit)
        {
            this.unit = unit;
            poolService = AppLoader.ServiceResolver.Resolve<IPoolService>();
        }

        /// <inheritdoc/>
        public void Detach()
        {
            unit = null;
        }

        /// <inheritdoc/>
        public void Reset()
        {
            if (animator == null)
            {
                return;
            }

            animator.Rebind();
        }

        /// <summary>
        /// Plays the death feedback.
        /// </summary>
        public void PlayDeath()
        {
            if (animator != null)
            {
                animator.SetTrigger(deathTrigger);
            }

            PlaySound(deathSound);

            OnDead?.Invoke();
        }

        /// <summary>
        /// Plays the hit feedback.
        /// </summary>
        public void PlayHit()
        {
            PlaySound(hitSound);
        }

        private void PlaySound(SoundEffect effect)
        {
            if (soundPrefab == null)
            {
                return;
            }

            var sound = poolService.Get(soundPrefab, unit.Root.position, Quaternion.identity);
            sound.transform.SetParent(unit.Root);
            sound.transform.localPosition = Vector3.zero;
            sound.Initialize(() => poolService.Release(soundPrefab, sound));
            sound.Play(effect);
        }
    }
}
