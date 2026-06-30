using Anchorpoint.Core.Units.Base;
using System;
using UnityEngine;

namespace Anchorpoint.App.Units.UnitComponents.Configs
{
    /// <summary>
    /// Builds a health component.
    /// </summary>
    [Serializable]
    public sealed class HealthConfig : UnitComponentConfigBase
    {
        [SerializeField]
        private int maxHealth = 100;

        /// <inheritdoc/>
        public override IUnitComponent Build() => new HealthComponent(maxHealth);
    }
}
