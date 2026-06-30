using TMPro;
using UnityEngine;

namespace Anchorpoint.Infrastructure.UI.Coordinates
{
    /// <summary>
    /// Shows the player's map coordinates as text.
    /// </summary>
    public sealed class CoordinateView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text label;

        /// <summary>
        /// Shows the given coordinates.
        /// </summary>
        public void Render(Vector2 coordinates)
        {
            label.text = $"X: {coordinates.x:F0} Y: {coordinates.y:F0}";
        }
    }
}
