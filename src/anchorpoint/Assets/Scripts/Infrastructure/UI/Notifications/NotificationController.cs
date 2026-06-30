using Anchorpoint.Infrastructure.Services.Interfaces;

namespace Anchorpoint.Infrastructure.UI.Notifications
{
    /// <summary>
    /// Ticks the notification service and updates its view.
    /// </summary>
    public sealed class NotificationController
    {
        private readonly INotificationService service;
        private readonly NotificationView view;

        /// <summary>
        /// Creates the controller and sets the message limit and lifetime.
        /// </summary>
        public NotificationController(INotificationService service, NotificationView view)
        {
            this.service = service;
            this.view = view;

            service.Configure(view.MaxCount, view.Duration);
        }

        /// <summary>
        /// Updates messages and refreshes the view.
        /// </summary>
        public void Tick(float dt)
        {
            service.Tick(dt);
            view.Render(service.Messages);
        }
    }
}
