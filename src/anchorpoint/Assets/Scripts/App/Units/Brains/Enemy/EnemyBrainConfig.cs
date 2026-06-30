using Anchorpoint.Core.Units.Base;
using System;

namespace Anchorpoint.App.Units.Brains.Enemy
{
    /// <summary>
    /// Builds an enemy brain.
    /// </summary>
    [Serializable]
    public class EnemyBrainConfig : UnitBrainComponentConfigBase
    {
        /// <inheritdoc/>
        public override IUnitComponent Build() => new EnemyBrainComponent();
    }
}
