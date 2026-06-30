using Anchorpoint.App.Events;
using Anchorpoint.App.Units.Brains;
using Anchorpoint.Infrastructure;
using Anchorpoint.Infrastructure.Environment;
using Anchorpoint.Infrastructure.Services;
using Anchorpoint.Infrastructure.Services.Base;
using Anchorpoint.Infrastructure.Services.Interfaces;
using Anchorpoint.Infrastructure.UI.Coordinates;
using Anchorpoint.Infrastructure.UI.Notifications;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Anchorpoint.App.SceneRoot
{
    /// <summary>
    /// Sets up the scene and drives all ticks.
    /// </summary>
    [DefaultExecutionOrder(Constants.RootExecutionOrder)]
    public sealed class GameSceneRoot : RootBase
    {
        [SerializeField]
        private UnitInstaller playerInstaller;

        [SerializeField]
        private EventCatalog eventCatalog;

        [Header("Base")]
        [SerializeField]
        private BaseZone baseZone;

        [Header("Spawn")]
        [SerializeField]
        private List<SpawnPoint> spawnPoints;

        [Header("Coordinates")]
        [SerializeField]
        private CoordinateView coordinateView;

        [Header("Notification")]
        [SerializeField]
        private NotificationView notificationView;

        private IUnitTickService unitTickService;
        private IPlayerContextService playerContext;
        private IEventService eventService;
        private NotificationController notificationController;
        private CoordinateController coordinateController;

        /// <inheritdoc/>
        protected override void Initialize(IServiceResolver serviceResolver, CancellationToken rootCt)
        {
            unitTickService = serviceResolver.Resolve<IUnitTickService>();
            playerContext = serviceResolver.Resolve<IPlayerContextService>();
            eventService = serviceResolver.Resolve<IEventService>();

            var playerUnit = playerInstaller.Initialize(serviceResolver.Resolve<IUnitFactoryService>());
            playerContext.SetPlayer(playerUnit);

            var notificationService = serviceResolver.Resolve<INotificationService>();
            notificationController = new NotificationController(notificationService, notificationView);

            coordinateController = new CoordinateController(serviceResolver.Resolve<ICoordinateService>(), coordinateView);

            baseZone.Initialze(serviceResolver.Resolve<IPlayerContextService>());

            foreach (var spawnPoint in spawnPoints)
            {
                spawnPoint.Initialze(serviceResolver.Resolve<ISpawnPointService>());
            }

            eventService.Begin(eventCatalog);
        }

        private void Update()
        {
            unitTickService.Tick(Time.deltaTime);
            eventService.Tick(Time.deltaTime);
            notificationController.Tick(Time.deltaTime);
            coordinateController.Tick(Time.deltaTime);
        }
    }
}
