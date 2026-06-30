using Anchorpoint.Core.Units.Base;
using System;
using UnityEngine;

namespace Anchorpoint.App.Units.UnitComponents.Configs
{
    /// <summary>
    /// Builds a look component.
    /// </summary>
    [Serializable]
    public sealed class LookConfig : UnitComponentConfigBase
    {
        [SerializeField]
        private float verticalClamp = 85f;

        [SerializeField]
        private float turnSpeed = 0.15f;

        /// <inheritdoc/>
        public override IUnitComponent Build() => new LookComponent(verticalClamp, turnSpeed);
    }
}
