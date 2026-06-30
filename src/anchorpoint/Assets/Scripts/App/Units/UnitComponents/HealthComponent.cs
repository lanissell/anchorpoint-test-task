using Anchorpoint.App.Units.UnitComponents.Views;
using Anchorpoint.Core.Units;
using Anchorpoint.Core.Units.Base;
using System;
using UnityEngine;

namespace Anchorpoint.App.Units.UnitComponents
{
    /// <summary>
    /// Tracks a unit's health.
    /// </summary>
    public sealed class HealthComponent : IUnitComponent
    {
        /// <summary>
        /// Max health.
        /// </summary>
        public int Max { get; }

        /// <summary>
        /// Current health.
        /// </summary>
        public int Current { get; private set; }

        /// <summary>
        /// True when health is zero.
        /// </summary>
        public bool IsDead => Current <= 0;

        private HealthComponentView view;
        private CharacterController cc;

        /// <summary>
        /// Creates the component with max health.
        /// </summary>
        public HealthComponent(int maxHealth)
        {
            Max = maxHealth;
            Current = maxHealth;
        }

        /// <inheritdoc/>
        public void Attach(Unit unit)
        {
            unit.TryGet(out view);
            cc = unit.CharacterController;
        }

        /// <inheritdoc/>
        public void Detach()
        {
        }

        /// <inheritdoc/>
        public void Reset()
        {
            Current = Max;

            if (cc != null)
            {
                cc.enabled = true;
            }
        }

        /// <summary>
        /// Deals damage to the unit.
        /// </summary>
        public void TakeDamage(int amount)
        {
            if (IsDead)
            {
                return;
            }

            Current = Math.Max(0, Current - amount);

            if (IsDead)
            {
                view?.PlayDeath();
                cc.enabled = false;
            }
            else
            {
                view?.PlayHit();
            }
        }

        /// <summary>
        /// Heals the unit, up to max.
        /// </summary>
        public void Heal(int amount)
        {
            if (IsDead)
            {
                return;
            }

            Current = Math.Min(Max, Current + amount);
        }
    }
}
