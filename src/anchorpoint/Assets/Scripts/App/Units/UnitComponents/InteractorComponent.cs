using Anchorpoint.Core.Interaction;
using Anchorpoint.Core.Units;
using Anchorpoint.Core.Units.Base;
using UnityEngine;

namespace Anchorpoint.App.Units.UnitComponents
{
    /// <summary>
    /// Lets the player pick up the nearest item in range.
    /// </summary>
    public sealed class InteractorComponent : IUnitComponent
    {
        private readonly float range;
        private readonly int pickupMask;
        private readonly Collider[] buffer = new Collider[8];

        private Unit unit;

        /// <summary>
        /// Creates the component with a reach and pickup layer mask.
        /// </summary>
        public InteractorComponent(float range, int pickupMask)
        {
            this.range = range;
            this.pickupMask = pickupMask;
        }

        /// <inheritdoc/>
        public void Attach(Unit attachedUnit)
        {
            unit = attachedUnit;
        }

        /// <inheritdoc/>
        public void Detach()
        {
            unit = null;
        }

        /// <inheritdoc/>
        public void Reset() { }

        /// <summary>
        /// Picks up the nearest item in range.
        /// </summary>
        public void TryCollectNearest()
        {
            int count = Physics.OverlapSphereNonAlloc(
                unit.Root.position,
                range,
                buffer,
                pickupMask,
                QueryTriggerInteraction.Collide);

            IPickup nearest = null;
            float nearestSqr = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                if (!buffer[i].TryGetComponent<IPickup>(out var pickup))
                {
                    continue;
                }

                float sqr = (buffer[i].transform.position - unit.Root.position).sqrMagnitude;

                if (sqr < nearestSqr)
                {
                    nearestSqr = sqr;
                    nearest = pickup;
                }
            }

            nearest?.Collect();
        }
    }
}
