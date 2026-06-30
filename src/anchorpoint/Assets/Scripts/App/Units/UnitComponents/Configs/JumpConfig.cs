using Anchorpoint.Core.Units.Base;
using System;
using UnityEngine;

namespace Anchorpoint.App.Units.UnitComponents.Configs
{
    /// <summary>
    /// Builds a jump component.
    /// </summary>
    [Serializable]
    public sealed class JumpConfig : UnitComponentConfigBase
    {
        [SerializeField]
        private float jumpHeight = 1.5f;

        /// <inheritdoc/>
        public override IUnitComponent Build() => new JumpComponent(jumpHeight);
    }
}
