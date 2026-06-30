using Anchorpoint.Core.Units.Base;
using System;
using UnityEngine;

namespace Anchorpoint.App.Units.UnitComponents.Configs
{
    /// <summary>
    /// Builds a gravity component.
    /// </summary>
    [Serializable]
    public sealed class GravityConfig : UnitComponentConfigBase
    {
        [SerializeField]
        private float gravity = -19.62f;

        /// <inheritdoc/>
        public override IUnitComponent Build() => new GravityComponent(gravity);
    }
}
