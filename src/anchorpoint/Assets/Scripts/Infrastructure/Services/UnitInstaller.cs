using Anchorpoint.Core.Units;
using Anchorpoint.Core.Units.Base;
using Anchorpoint.Infrastructure.Services.Interfaces;
using System.Linq;
using UnityEngine;

namespace Anchorpoint.Infrastructure.Services
{
    /// <summary>
    /// Builds a unit from its definition in the scene.
    /// </summary>
    public class UnitInstaller : MonoBehaviour
    {
        [SerializeField]
        private UnitDefinition definition;

        [SerializeField]
        private CharacterController characterController;

        [SerializeField]
        private  Transform gazeHolder;

        /// <summary>
        /// Builds and registers the unit.
        /// </summary>
        public Unit Initialize(IUnitFactoryService factory)
        {
            var views = GetComponentsInChildren<MonoBehaviour>(true)
                .OfType<IUnitComponent>()
                .ToList();

            return factory.Create(definition, characterController, gazeHolder, views);
        }
    }
}
