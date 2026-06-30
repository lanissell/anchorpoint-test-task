using Anchorpoint.Core.Units.Base;
using System;
using UnityEngine;

namespace Anchorpoint.App.Units.UnitComponents.Configs
{
    /// <summary>
    /// Builds a movement component.
    /// </summary>
    [Serializable]
    public sealed class MovementConfig : UnitComponentConfigBase
    {
        [SerializeField]
        private float walkSpeed = 5f;

        [SerializeField]
        private float sprintSpeed = 8f;

        /// <inheritdoc/>
        public override IUnitComponent Build() => new MovementComponent(walkSpeed, sprintSpeed);
    }
}
