using System;

namespace Anchorpoint.Core.Units.Base
{
    /// <summary>
    /// Serialized settings that build a runtime component.
    /// </summary>
    [Serializable]
    public abstract class UnitComponentConfigBase
    {
        /// <summary>
        /// Builds the runtime component.
        /// </summary>
        public abstract IUnitComponent Build();
    }
}
