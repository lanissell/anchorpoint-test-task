using Anchorpoint.App.Events;
using Anchorpoint.Core.Events.Base;
using Anchorpoint.Infrastructure.Services.Interfaces;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Anchorpoint.Infrastructure.Services.Implementations
{
    /// <summary>
    /// Fires random events from the catalog and runs them.
    /// </summary>
    public sealed class EventService : IEventService
    {
        private sealed class ActiveEvent
        {
            public IGameEvent Event;
            public GameEventConfigBase Config;
            public bool Finished;
            public float CleanupTimer;
        }

        private readonly IPlayerContextService playerContext;
        private readonly INotificationService notificationService;
        private readonly List<ActiveEvent> active = new List<ActiveEvent>();
        private readonly List<GameEventConfigBase> candidates = new List<GameEventConfigBase>();

        private EventCatalog catalog;
        private float timer;

        /// <summary>
        /// Creates the service with the player context and notifications.
        /// </summary>
        public EventService(IPlayerContextService playerContext, INotificationService notificationService)
        {
            this.playerContext = playerContext;
            this.notificationService = notificationService;
        }

        /// <inheritdoc/>
        public void Begin(EventCatalog catalog)
        {
            this.catalog = catalog;
            ResetTimer();
        }

        /// <inheritdoc/>
        public void Tick(float dt)
        {
            for (int i = active.Count - 1; i >= 0; i--)
            {
                var entry = active[i];

                if (!entry.Finished)
                {
                    if (entry.Event.Tick(dt))
                    {
                        continue;
                    }

                    entry.Finished = true;

                    var endMessage = entry.Event.WasSuccess ? entry.Config.SuccessMessage : entry.Config.FailureMessage;

                    if (!string.IsNullOrEmpty(endMessage))
                    {
                        notificationService.Push(endMessage);
                    }

                    entry.CleanupTimer = entry.Event.WasSuccess ? SuccessCleanupDelay : 0f;
                }
                else
                {
                    entry.CleanupTimer -= dt;
                }

                if (entry.CleanupTimer <= 0f)
                {
                    entry.Event.Cleanup();
                    active.RemoveAt(i);
                }
            }

            if (catalog == null)
            {
                return;
            }

            timer -= dt;

            if (timer > 0f)
            {
                return;
            }

            FireRandom();
            ResetTimer();
        }

        private void FireRandom()
        {
            var current = playerContext.Current;

            candidates.Clear();

            foreach (var config in catalog.Events)
            {
                if (config != null && config.Context == current && !IsActive(config))
                {
                    candidates.Add(config);
                }
            }

            if (candidates.Count == 0)
            {
                return;
            }

            var chosen = candidates[Random.Range(0, candidates.Count)];
            var ev = chosen.Build();
            ev.Begin();
            active.Add(new ActiveEvent { Event = ev, Config = chosen });

            var desc = ev.SpawnDescription;
            var startMsg = string.IsNullOrEmpty(desc)
                ? chosen.StartMessage
                : $"{chosen.StartMessage} {desc}";

            if (!string.IsNullOrEmpty(startMsg))
            {
                notificationService.Push(startMsg);
            }
        }

        private bool IsActive(GameEventConfigBase config)
        {
            foreach (var entry in active)
            {
                if (ReferenceEquals(entry.Config, config))
                {
                    return true;
                }
            }

            return false;
        }

        private float SuccessCleanupDelay => catalog != null ? catalog.SuccessCleanupDelay : 0f;

        private void ResetTimer()
        {
            timer = Random.Range(catalog.MinInterval, catalog.MaxInterval);
        }
    }
}
