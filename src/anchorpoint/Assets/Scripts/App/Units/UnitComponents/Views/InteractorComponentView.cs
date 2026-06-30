using Anchorpoint.Core.Units;
using Anchorpoint.Core.Units.Base;
using Anchorpoint.Infrastructure;
using Anchorpoint.Infrastructure.Services;
using Anchorpoint.Infrastructure.Services.Interfaces;
using Anchorpoint.Infrastructure.Services.PoolEntries;
using UnityEngine;

namespace Anchorpoint.App.Units.UnitComponents.Views
{
    /// <summary>
    /// Plays the pickup sound.
    /// </summary>
    public sealed class InteractorComponentView : MonoBehaviour, IUnitComponent
    {
        [Header("Sound")]
        [SerializeField]
        private SoundEntry soundPrefab;

        [SerializeField]
        private SoundEffect sound;

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
        /// Plays the pickup sound.
        /// </summary>
        public void Play()
        {
            if (soundPrefab != null)
            {
                var entry = poolService.Get(soundPrefab, unit.Root.position, Quaternion.identity);
                entry.transform.SetParent(unit.Root);
                entry.transform.localPosition = Vector3.zero;
                entry.Initialize(() => poolService.Release(soundPrefab, entry));
                entry.Play(sound);
            }
        }
    }
}
