using Anchorpoint.Core.Events;
using Anchorpoint.Infrastructure.Services.Interfaces;
using UnityEngine;

namespace Anchorpoint.Infrastructure.Environment
{
    /// <summary>
    /// A spot where events can spawn objects.
    /// </summary>
    [ExecuteAlways]
    public sealed class SpawnPoint : MonoBehaviour
    {
        /// <summary>
        /// The area this point belongs to.
        /// </summary>
        [field: SerializeField]
        public EventContext Area { get; private set; }

        /// <summary>
        /// This point's world position.
        /// </summary>
        public Vector3 Position => transform.position;

        private ISpawnPointService spawnPointService;

        /// <summary>
        /// Sets up the point and registers it.
        /// </summary>
        public void Initialze(ISpawnPointService spawnPointService)
        {
            this.spawnPointService = spawnPointService;
            spawnPointService.Register(this);
        }

        private void OnEnable()
        {
            spawnPointService?.Register(this);
        }

        private void Update()
        {
            if (Application.isPlaying)
            {
                return;
            }

            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit))
            {
                transform.position = hit.point;
            }
        }

        private void OnDisable()
        {
            spawnPointService?.Unregister(this);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Area == EventContext.OnBase ? Color.cyan : Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
}
