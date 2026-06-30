using System;

namespace Anchorpoint.Core.Units.Base
{
    /// <summary>
    /// Serialized settings that build a runtime brain.
    /// </summary>
    [Serializable]
    public abstract class UnitBrainComponentConfigBase
    {
        /// <summary>
        /// Builds the runtime brain.
        /// </summary>
        public abstract IUnitComponent Build();
    }
}
