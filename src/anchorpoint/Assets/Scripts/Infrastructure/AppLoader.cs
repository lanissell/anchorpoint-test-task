using Anchorpoint.Infrastructure.Services.Base;
using Anchorpoint.Infrastructure.Services.Implementations;
using Anchorpoint.Infrastructure.Services.Interfaces;
using UnityEngine;

namespace Anchorpoint.Infrastructure
{
    /// <summary>
    /// App entry point that registers all services.
    /// </summary>
    [DefaultExecutionOrder(Constants.AppLoaderExecutionOrder)]
    public class AppLoader : MonoBehaviour
    {
        /// <summary>
        /// Access to all services.
        /// </summary>
        public static IServiceResolver ServiceResolver
        {
            get
            {
                ValidateServiceLocator();
                return serviceLocator;
            }
        }

        [SerializeField]
        private int poolCapacity = 8;

        private static ServiceLocator serviceLocator;

        private static void ValidateServiceLocator()
        {
            if (serviceLocator != null)
            {
                return;
            }

            var appLoader = Instantiate(Resources.Load<AppLoader>(nameof(AppLoader)));
            appLoader.Awake();
        }

        private void Awake()
        {
            if (serviceLocator != null)
            {
                return;
            }

            serviceLocator = new();
            DontDestroyOnLoad(gameObject);
            Initialize(serviceLocator);
        }

        private void OnApplicationQuit()
        {
            Deinitialize(serviceLocator);
        }

        private void Initialize(ServiceLocator serviceLocator)
        {
            serviceLocator.Register<IInputActionsProviderService>(new InputActionsProviderService());

            var unitTickService = new UnitTickService();
            serviceLocator.Register<IUnitTickService>(unitTickService);

            var unitFactoryService = new UnitFactoryService(unitTickService);
            serviceLocator.Register<IUnitFactoryService>(unitFactoryService);

            var playerContext = new PlayerContextService();
            serviceLocator.Register<IPlayerContextService>(playerContext);

            serviceLocator.Register<ISpawnPointService>(new SpawnPointService(playerContext));

            var notificationService = new NotificationService();
            serviceLocator.Register<INotificationService>(notificationService);

            serviceLocator.Register<IEventService>(new EventService(playerContext, notificationService));

            serviceLocator.Register<IPoolService>(new PoolService(poolCapacity, transform));

            serviceLocator.Register<ICoordinateService>(new CoordinateService(playerContext));
        }

        private void Deinitialize(ServiceLocator serviceLocator)
        {
            serviceLocator.DeInitialize();
        }
    }
}