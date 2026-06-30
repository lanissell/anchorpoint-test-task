using Anchorpoint.Core.Units.Base;
using System;
using UnityEngine;

namespace Anchorpoint.App.Units.UnitComponents.Configs
{
    /// <summary>
    /// Builds a ranged attack component.
    /// </summary>
    [Serializable]
    public sealed class RangedAttackConfig : UnitComponentConfigBase
    {
        [SerializeField]
        private int damage = 5;

        [SerializeField]
        private float range = 15f;

        [SerializeField]
        private float cooldown = 0.5f;

        [SerializeField]
        private LayerMask hitMask;

        [SerializeField]
        private bool automatic;

        /// <inheritdoc/>
        public override IUnitComponent Build() => new RangedAttackComponent(damage, range, cooldown, hitMask.value, automatic);
    }
}
