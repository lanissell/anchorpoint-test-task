using Anchorpoint.Core.Units.Base;
using System;
using UnityEngine;

namespace Anchorpoint.App.Units.UnitComponents.Configs
{
    /// <summary>
    /// Builds a melee attack component.
    /// </summary>
    [Serializable]
    public sealed class MeleeAttackConfig : UnitComponentConfigBase
    {
        [SerializeField]
        private int damage = 10;

        [SerializeField]
        private float range = 2f;

        [SerializeField]
        private float cooldown = 1f;

        [SerializeField]
        private LayerMask targetMask;

        /// <inheritdoc/>
        public override IUnitComponent Build() => new MeleeAttackComponent(damage, range, cooldown, targetMask.value);
    }
}
