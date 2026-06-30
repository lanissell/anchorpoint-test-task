using Anchorpoint.Core.Units.Base;
using System;
using UnityEngine;

namespace Anchorpoint.App.Units.UnitComponents.Configs
{
    /// <summary>
    /// Builds an interactor component.
    /// </summary>
    [Serializable]
    public sealed class InteractorConfig : UnitComponentConfigBase
    {
        [SerializeField]
        private float range = 2.5f;

        [SerializeField]
        private LayerMask pickupMask;

        /// <inheritdoc/>
        public override IUnitComponent Build() => new InteractorComponent(range, pickupMask.value);
    }
}
