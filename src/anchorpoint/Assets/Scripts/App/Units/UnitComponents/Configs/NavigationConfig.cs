using Anchorpoint.Core.Units.Base;
using System;
using UnityEngine;

namespace Anchorpoint.App.Units.UnitComponents.Configs
{
    /// <summary>
    /// Builds a navigation component.
    /// </summary>
    [Serializable]
    public sealed class NavigationConfigBase : UnitComponentConfigBase
    {
        [SerializeField]
        private float stoppingDistance = 1.5f;

        /// <inheritdoc/>
        public override IUnitComponent Build() => new NavigationComponent(stoppingDistance);
    }
}
