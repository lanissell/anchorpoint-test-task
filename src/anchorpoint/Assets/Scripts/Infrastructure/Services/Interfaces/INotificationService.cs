using System.Collections.Generic;

namespace Anchorpoint.Infrastructure.Services.Interfaces
{
    /// <summary>
    /// Holds on-screen messages that expire over time.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Current messages, newest first.
        /// </summary>
        IReadOnlyList<string> Messages { get; }

        /// <summary>
        /// Adds a message.
        /// </summary>
        void Push(string message);

        /// <summary>
        /// Drops expired messages.
        /// </summary>
        void Tick(float dt);

        /// <summary>
        /// Sets the message limit and how long each lasts.
        /// </summary>
        void Configure(int maxCount, float duration);
    }
}
