using Anchorpoint.Core.Units.Base;
using System.Collections.Generic;
using UnityEngine;

namespace Anchorpoint.Core.Units
{
    /// <summary>
    /// Asset listing which components a unit type has and their settings.
    /// </summary>
    [CreateAssetMenu(fileName = "UnitDefinition", menuName = "Anchorpoint/Unit Definition")]
    public sealed class UnitDefinition : ScriptableObject
    {
        /// <summary>
        /// The unit's brain, built and attached first.
        /// </summary>
        [field: SerializeReference, SubclassSelector]
        public UnitBrainComponentConfigBase BrainConfig { get; private set; }

        [SerializeReference, SubclassSelector]
        private List<UnitComponentConfigBase> componentConfigs = new List<UnitComponentConfigBase>();

        /// <summary>
        /// Components to build when spawning this unit, in order.
        /// </summary>
        public IReadOnlyList<UnitComponentConfigBase> ComponentConfigs => componentConfigs;
    }
}
