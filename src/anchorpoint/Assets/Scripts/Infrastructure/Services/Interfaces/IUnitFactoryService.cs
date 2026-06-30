using Anchorpoint.Core.Units;
using Anchorpoint.Core.Units.Base;
using System.Collections.Generic;
using UnityEngine;

namespace Anchorpoint.Infrastructure.Services.Interfaces
{
    /// <summary>
    /// Builds units from a definition and registers them.
    /// </summary>
    public interface IUnitFactoryService
    {
        /// <summary>
        /// Builds and registers a unit from the definition.
        /// </summary>
        Unit Create(UnitDefinition definition,
            CharacterController characterController,
            Transform gazeHolder,
            IReadOnlyList<IUnitComponent> sceneComponents);
    }
}
