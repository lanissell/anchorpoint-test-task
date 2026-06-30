using Anchorpoint.App.Units.UnitComponents.Views;
using Anchorpoint.Core.Units;
using Anchorpoint.Core.Units.Base;
using Anchorpoint.Infrastructure.Services.Interfaces;
using System.Linq;
using UnityEngine;

namespace Anchorpoint.App.Units.UnitComponents
{
    /// <summary>
    /// Hits a nearby target when off cooldown.
    /// </summary>
    public sealed class MeleeAttackComponent : IUnitComponent
    {
        private readonly int damage;
        private readonly float range;
        private readonly float cooldown;
        private readonly int targetMask;
        private readonly Collider[] buffer = new Collider[8];

        private IUnitTickService unitTickService;

        private Unit unit;
        private MeleeAttackComponentView view;
        private float nextAttackTime;

        /// <summary>
        /// Creates the component with damage, reach, cooldown, and target mask.
        /// </summary>
        public MeleeAttackComponent(int damage, float range, float cooldown, int targetMask)
        {
            this.damage = damage;
            this.range = range;
            this.cooldown = cooldown;
            this.targetMask = targetMask;
        }

        /// <inheritdoc/>
        public void Attach(Unit attachedUnit)
        {
            unit = attachedUnit;
            unit.TryGet(out view);

            unitTickService = AppLoader.ServiceResolver.Resolve<IUnitTickService>();
        }

        /// <inheritdoc/>
        public void Detach()
        {
            unit = null;
        }

        /// <inheritdoc/>
        public void Reset()
        {
            nextAttackTime = 0f;
        }

        /// <summary>
        /// Swings only if a target is in range; returns true when it hits.
        /// </summary>
        public bool TryAttack()
        {
            if (Time.time < nextAttackTime || !TryGetVictim(out var victim))
            {
                return false;
            }

            PerformSwing();
            victim.TakeDamage(damage);
            return true;
        }

        /// <summary>
        /// Always swings when off cooldown, dealing damage if a target is in range.
        /// </summary>
        public void Swing()
        {
            if (Time.time < nextAttackTime)
            {
                return;
            }

            PerformSwing();

            if (TryGetVictim(out var victim))
            {
                victim.TakeDamage(damage);
            }
        }

        private void PerformSwing()
        {
            view?.Play();
            nextAttackTime = Time.time + cooldown;
        }

        private bool TryGetVictim(out HealthComponent victim)
        {
            int count = Physics.OverlapSphereNonAlloc(unit.Root.position, range, buffer, targetMask);

            for (int i = 0; i < count; i++)
            {
                var hit = buffer[i].transform;

                if (hit == unit.Root || hit.IsChildOf(unit.Root))
                {
                    continue;
                }

                var target = unitTickService.Units.FirstOrDefault(u => u.Root == hit);

                if (target != null && target.TryGet(out victim))
                {
                    return true;
                }
            }

            victim = null;
            return false;
        }
    }
}
