using Anchorpoint.Core.Units;
using Anchorpoint.Core.Units.Base;
using Anchorpoint.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Anchorpoint.Infrastructure.Services.Implementations
{
    /// <summary>
    /// Builds units from a definition and registers them.
    /// </summary>
    public sealed class UnitFactoryService : IUnitFactoryService
    {
        private readonly IUnitTickService unitTickService;

        /// <summary>
        /// Creates the factory with the unit tick service.
        /// </summary>
        public UnitFactoryService(IUnitTickService unitTickService)
        {
            this.unitTickService = unitTickService ?? throw new ArgumentNullException(nameof(unitTickService));
        }

        /// <inheritdoc/>
        public Unit Create(UnitDefinition definition,
            CharacterController characterController,
            Transform gazeHolder,
            IReadOnlyList<IUnitComponent> sceneComponents)
        {
            var unit = new Unit(characterController, gazeHolder);

            unit.AddSceneComponents(sceneComponents);

            unit.Add(definition.BrainConfig.Build());

            foreach (var config in definition.ComponentConfigs)
            {
                if (config != null)
                {
                    unit.Add(config.Build());
                }
            }

            unitTickService.Register(unit);
            return unit;
        }
    }
}
