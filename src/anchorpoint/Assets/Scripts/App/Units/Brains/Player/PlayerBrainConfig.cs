using Anchorpoint.Core.Units.Base;
using System;

namespace Anchorpoint.App.Units.Brains.Player
{
    /// <summary>
    /// Builds a player brain.
    /// </summary>
    [Serializable]
    public class PlayerBrainConfig : UnitBrainComponentConfigBase
    {
        /// <inheritdoc/>
        public override IUnitComponent Build() => new PlayerBrainComponent();
    }
}
