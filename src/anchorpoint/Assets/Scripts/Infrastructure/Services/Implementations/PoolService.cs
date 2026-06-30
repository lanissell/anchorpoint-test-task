using Anchorpoint.Infrastructure.Services.Interfaces;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Anchorpoint.Infrastructure.Services.Implementations
{
    /// <summary>
    /// Reuses prefab instances using a pool per prefab.
    /// </summary>
    public sealed class PoolService : IPoolService
    {
        private readonly int capacity;
        private readonly Transform root;
        private readonly Dictionary<object, object> pools = new();

        /// <summary>
        /// Creates the pool with instances parented under root.
        /// </summary>
        public PoolService(int capacity, Transform root)
        {
            this.capacity = capacity;
            this.root = root;
        }

        /// <inheritdoc/>
        public T Get<T>(T prefab, Vector3 position, Quaternion rotation) where T : MonoBehaviour
        {
            var instance = GetPool(prefab).Get();
            instance.transform.SetPositionAndRotation(position, rotation);

            instance.gameObject.SetActive(true);

            return instance;
        }

        /// <inheritdoc/>
        public void Release<T>(T prefab, T instance) where T : MonoBehaviour
        {
            GetPool(prefab).Release(instance);
        }

        private ObjectPool<T> GetPool<T>(T prefab) where T : MonoBehaviour
        {
            if (pools.TryGetValue(prefab, out var existing))
            {
                return (ObjectPool<T>)existing;
            }

            var pool = new ObjectPool<T>(
                createFunc: () => Object.Instantiate(prefab, root),
                actionOnGet: _ => {},
                actionOnRelease: p =>
                {
                    p.transform.SetParent(root);
                    p.gameObject.SetActive(false);
                },
                actionOnDestroy: p => Object.Destroy(p.gameObject),
                collectionCheck: false,
                defaultCapacity: capacity);

            pools[prefab] = pool;
            return pool;
        }
    }
}
