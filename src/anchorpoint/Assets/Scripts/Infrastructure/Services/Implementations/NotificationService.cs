using Anchorpoint.Infrastructure.Services.Interfaces;
using System.Collections.Generic;

namespace Anchorpoint.Infrastructure.Services.Implementations
{
    /// <summary>
    /// Holds on-screen messages that expire over time.
    /// </summary>
    public sealed class NotificationService : INotificationService
    {
        private sealed class Entry
        {
            public string Message;
            public float Remaining;
        }

        private readonly List<Entry> entries = new List<Entry>();
        private readonly List<string> messages = new List<string>();

        private int maxCount = 3;
        private float duration = 3f;

        /// <inheritdoc/>
        public IReadOnlyList<string> Messages => messages;

        /// <inheritdoc/>
        public void Configure(int maxCount, float duration)
        {
            this.maxCount = maxCount < 1 ? 1 : maxCount;
            this.duration = duration;

            TrimToMax();
            RebuildMessages();
        }

        /// <inheritdoc/>
        public void Push(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            entries.Insert(0, new Entry { Message = message, Remaining = duration });

            TrimToMax();
            RebuildMessages();
        }

        /// <inheritdoc/>
        public void Tick(float dt)
        {
            bool changed = false;

            for (int i = entries.Count - 1; i >= 0; i--)
            {
                entries[i].Remaining -= dt;

                if (entries[i].Remaining <= 0f)
                {
                    entries.RemoveAt(i);
                    changed = true;
                }
            }

            if (changed)
            {
                RebuildMessages();
            }
        }

        private void TrimToMax()
        {
            while (entries.Count > maxCount)
            {
                entries.RemoveAt(entries.Count - 1);
            }
        }

        private void RebuildMessages()
        {
            messages.Clear();

            for (int i = 0; i < entries.Count; i++)
            {
                messages.Add(entries[i].Message);
            }
        }
    }
}
