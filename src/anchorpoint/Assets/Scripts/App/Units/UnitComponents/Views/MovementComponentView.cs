using Anchorpoint.Core.Units;
using Anchorpoint.Core.Units.Base;
using Anchorpoint.Infrastructure.Services;
using Anchorpoint.Infrastructure.Services.Interfaces;
using Anchorpoint.Infrastructure.Services.PoolEntries;
using UnityEngine;

namespace Anchorpoint.App.Units.UnitComponents.Views
{
    /// <summary>
    /// Plays movement animation and footstep sounds.
    /// </summary>
    public sealed class MovementComponentView : MonoBehaviour, IUnitComponent
    {
        [Header("Animation")]
        [SerializeField]
        private string walkBool = "IsWalk";

        [SerializeField]
        private string runBool = "IsRun";

        [SerializeField]
        private Animator animator;

        [Header("Sound")]
        [SerializeField]
        private SoundEntry soundPrefab;

        [SerializeField]
        private SoundEffect footstep;

        private Unit unit;
        private IPoolService poolService;
        private SoundEntry sound;

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
            poolService = null;
            sound = null;
        }

        /// <inheritdoc/>
        public void Reset()
        {
            SetLocomotion(false, false);
        }

        /// <summary>
        /// Sets the walk/run animation state.
        /// </summary>
        public void SetLocomotion(bool walking, bool running)
        {
            animator?.SetBool(walkBool, walking);
            animator?.SetBool(runBool, running);

            if (!walking && !running)
            {
                sound?.Stop();
                sound = null;
            }
        }

        private void OnStep()
        {
            if (sound != null && !sound.Finished)
            {
                return;
            }

            if (soundPrefab == null)
            {
                return;
            }

            var entry = poolService.Get(soundPrefab, unit.Root.position, Quaternion.identity);
            sound = entry;
            entry.transform.SetParent(unit.Root);
            entry.transform.localPosition = Vector3.zero;
            entry.Initialize(() => poolService.Release(soundPrefab, entry));
            entry.Play(footstep, () => sound = null);
        }
    }
}
