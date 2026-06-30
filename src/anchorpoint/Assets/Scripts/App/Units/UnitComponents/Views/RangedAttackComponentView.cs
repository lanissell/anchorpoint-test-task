using Anchorpoint.Core.Units;
using Anchorpoint.Core.Units.Base;
using Anchorpoint.Infrastructure.Services;
using Anchorpoint.Infrastructure.Services.Interfaces;
using Anchorpoint.Infrastructure.Services.PoolEntries;
using UnityEngine;

namespace Anchorpoint.App.Units.UnitComponents.Views
{
    /// <summary>
    /// Plays shot and impact feedback.
    /// </summary>
    public sealed class RangedAttackComponentView : MonoBehaviour, IUnitComponent
    {
        [Header("Animation")]
        [SerializeField]
        private string trigger = "IsAttack";

        [SerializeField]
        private Animator animator;

        [Header("Sound")]
        [SerializeField]
        private SoundEntry soundPrefab;

        [SerializeField]
        private SoundEffect sound;

        [Header("Particle")]
        [SerializeField]
        private Transform muzzleAnchor;

        [SerializeField]
        private ParticleEntry particlePrefab;

        [Header("Impact")]
        [SerializeField]
        private DecalEntry bulletHolePrefab;

        [SerializeField]
        private float bulletHoleOffset = 0.01f;

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
        public void Reset() { }

        /// <summary>
        /// Plays the shot feedback.
        /// </summary>
        public void Play()
        {
            animator?.SetTrigger(trigger);

            if (soundPrefab != null)
            {
                var entry = poolService.Get(soundPrefab, unit.Root.position, Quaternion.identity);
                entry.transform.SetParent(unit.Root);
                entry.transform.localPosition = Vector3.zero;
                entry.Initialize(() => poolService.Release(soundPrefab, entry));
                entry.Play(sound);
            }

            if (particlePrefab != null && muzzleAnchor != null)
            {
                var fx = poolService.Get(particlePrefab, muzzleAnchor.position, muzzleAnchor.rotation);
                fx.transform.SetParent(muzzleAnchor);
                fx.transform.localPosition = Vector3.zero;
                fx.transform.localRotation = Quaternion.identity;
                fx.Initialize(() => poolService.Release(particlePrefab, fx));
                fx.Play();
            }
        }

        /// <summary>
        /// Plays the hit feedback and adds a bullet hole on non-units.
        /// </summary>
        public void PlayImpact(RaycastHit hit, bool hitUnit)
        {
            var rotation = Quaternion.LookRotation(hit.normal);

            if (!hitUnit && bulletHolePrefab != null)
            {
                var position = hit.point + hit.normal * bulletHoleOffset;
                var hole = poolService.Get(bulletHolePrefab, position, rotation);
                hole.transform.SetParent(hit.transform);
                hole.Initialize(() => poolService.Release(bulletHolePrefab, hole));
                hole.Play();
            }
        }
    }
}
