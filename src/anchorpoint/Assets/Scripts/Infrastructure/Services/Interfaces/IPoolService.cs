using UnityEngine;

namespace Anchorpoint.Infrastructure.Services.Interfaces
{
    /// <summary>
    /// Reuses prefab instances by their component type.
    /// </summary>
    public interface IPoolService
    {
        /// <summary>
        /// Gets a pooled instance at the given pose.
        /// </summary>
        T Get<T>(T prefab, Vector3 position, Quaternion rotation) where T : MonoBehaviour;

        /// <summary>
        /// Returns an instance to its pool.
        /// </summary>
        void Release<T>(T prefab, T instance) where T : MonoBehaviour;
    }
}
