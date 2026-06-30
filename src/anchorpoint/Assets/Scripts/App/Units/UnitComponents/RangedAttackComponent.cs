using Anchorpoint.App.Units.UnitComponents.Views;
using Anchorpoint.Core.Units;
using Anchorpoint.Core.Units.Base;
using Anchorpoint.Infrastructure.Services.Interfaces;
using System.Linq;
using UnityEngine;

namespace Anchorpoint.App.Units.UnitComponents
{
    /// <summary>
    /// Shoots a ray forward and damages the first target hit.
    /// </summary>
    public sealed class RangedAttackComponent : IUnitComponent
    {
        private readonly int damage;
        private readonly float range;
        private readonly float cooldown;
        private readonly int hitMask;
        private readonly bool automatic;

        private IUnitTickService unitTickService;

        private Unit unit;
        private RangedAttackComponentView view;
        private float nextShotTime;

        /// <summary>
        /// True if the weapon fires while the trigger is held.
        /// </summary>
        public bool IsAutomatic => automatic;

        /// <summary>
        /// Creates the component with damage, range, cooldown, hit mask, and auto-fire.
        /// </summary>
        public RangedAttackComponent(int damage, float range, float cooldown, int hitMask, bool automatic)
        {
            this.damage = damage;
            this.range = range;
            this.cooldown = cooldown;
            this.hitMask = hitMask;
            this.automatic = automatic;
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
            nextShotTime = 0f;
        }

        /// <summary>
        /// Fires only if a target is hit; returns true when it deals damage.
        /// </summary>
        public bool TryAttack()
        {
            if (Time.time < nextShotTime || !TryRaycast(out var hit) || !TryResolveVictim(hit, out var victim))
            {
                return false;
            }

            PerformShot();
            view?.PlayImpact(hit, true);
            victim.TakeDamage(damage);
            return true;
        }

        /// <summary>
        /// Always fires when off cooldown, dealing damage if a target is hit.
        /// </summary>
        public void Shoot()
        {
            if (Time.time < nextShotTime)
            {
                return;
            }

            PerformShot();

            if (!TryRaycast(out var hit))
            {
                return;
            }

            bool hitUnit = TryResolveVictim(hit, out var victim);
            view?.PlayImpact(hit, hitUnit);

            if (hitUnit)
            {
                victim.TakeDamage(damage);
            }
        }

        private void PerformShot()
        {
            view?.Play();
            nextShotTime = Time.time + cooldown;
        }

        private bool TryRaycast(out RaycastHit hit)
        {
            var direction = unit.GazeHolder != null ? unit.GazeHolder.forward : unit.Root.forward;
            var muzzle = unit.GazeHolder != null ? unit.GazeHolder.position : unit.Root.position;

            var origin = muzzle + direction * (unit.CharacterController.radius + 0.1f);

            return Physics.Raycast(origin, direction, out hit, range, hitMask, QueryTriggerInteraction.Ignore);
        }

        private bool TryResolveVictim(in RaycastHit hit, out HealthComponent victim)
        {
            var hitTransform = hit.transform;
            var target = unitTickService.Units.FirstOrDefault(u => u.Root == hitTransform && u != unit);

            if (target != null && target.TryGet(out victim))
            {
                return true;
            }

            victim = null;
            return false;
        }
    }
}
