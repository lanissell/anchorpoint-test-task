using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Anchorpoint.Infrastructure.UI.Notifications
{
    /// <summary>
    /// Shows notification messages as a stacked text list.
    /// </summary>
    public sealed class NotificationView : MonoBehaviour
    {
        [SerializeField]
        private RectTransform container;

        [SerializeField]
        private TMP_Text itemPrefab;

        [field: SerializeField]
        public int MaxCount = 3;

        [field: SerializeField]
        public float Duration = 3f;

        private readonly List<TMP_Text> items = new List<TMP_Text>();

        /// <summary>
        /// Shows the given messages.
        /// </summary>
        public void Render(IReadOnlyList<string> messages)
        {
            EnsureCapacity(messages.Count);

            for (int i = 0; i < items.Count; i++)
            {
                if (i < messages.Count)
                {
                    items[i].text = messages[i];
                    items[i].gameObject.SetActive(true);
                }
                else
                {
                    items[i].gameObject.SetActive(false);
                }
            }
        }

        private void EnsureCapacity(int count)
        {
            while (items.Count < count)
            {
                var item = Instantiate(itemPrefab, container);
                items.Add(item);
            }
        }
    }
}
