using Anchorpoint.Core.Units;
using Anchorpoint.Core.Units.Base;
using UnityEngine;
using UnityEngine.AI;

namespace Anchorpoint.App.Units.UnitComponents
{
    /// <summary>
    /// Turns a NavMeshAgent's path into a local move direction.
    /// </summary>
    public sealed class NavigationComponent : IUnitComponent, IUnitTick
    {
        private readonly float stoppingDistance;

        private Unit unit;
        private NavMeshAgent agent;

        /// <summary>
        /// Where the unit wants to move, in local space (x = strafe, y = forward).
        /// </summary>
        public Vector2 DesiredLocalAxis { get; private set; }

        /// <summary>
        /// Distance left to the destination.
        /// </summary>
        public float RemainingDistance => agent != null ? agent.remainingDistance : 0f;

        /// <summary>
        /// Creates the component with a stopping distance.
        /// </summary>
        public NavigationComponent(float stoppingDistance)
        {
            this.stoppingDistance = stoppingDistance;
        }

        /// <inheritdoc/>
        public void Attach(Unit attachedUnit)
        {
            unit = attachedUnit;
            agent = attachedUnit.Root.GetComponent<NavMeshAgent>();
            agent.stoppingDistance = stoppingDistance;

            // An enabled agent ignores transform.position and keeps its own internal position
            // (stale after pooling), so place it on the NavMesh at the spawn pose via Warp.
            agent.enabled = true;
            agent.Warp(attachedUnit.Root.position);
        }

        /// <inheritdoc/>
        public void Detach()
        {
            agent = null;
            unit = null;
        }

        /// <inheritdoc/>
        public void Reset()
        {
            agent.enabled = true;
            agent.ResetPath();
            DesiredLocalAxis = Vector2.zero;
        }

        /// <summary>
        /// Sets where to walk to.
        /// </summary>
        public void SetDestination(Vector3 worldPos)
        {
            if (!agent.enabled)
            {
                return;
            }

            agent?.SetDestination(worldPos);
        }

        /// <inheritdoc/>
        public void Tick(float dt)
        {
            if (agent == null)
            {
                DesiredLocalAxis = Vector2.zero;
                return;
            }

            if (unit.TryGet(out MovementComponent movement))
            {
                agent.speed = movement.CurrentSpeed;
            }

            if (unit.TryGet(out LookComponent look))
            {
                agent.angularSpeed = look.TurnSpeed;
            }

            if (unit.TryGet(out HealthComponent health))
            {
                agent.enabled = !health.IsDead;
            }

            agent.height = unit.CharacterController.height;
            agent.radius = unit.CharacterController.radius;

            if (!agent.isOnNavMesh || !agent.enabled)
            {
                DesiredLocalAxis = Vector2.zero;
                return;
            }

            var drift = agent.nextPosition - unit.Root.position;
            drift.y = 0f;

            if (drift.sqrMagnitude > agent.radius * agent.radius
                && NavMesh.SamplePosition(unit.Root.position, out var hit, agent.radius, NavMesh.AllAreas))
            {
                agent.nextPosition = hit.position;
            }

            if (!agent.hasPath || RemainingDistance <= stoppingDistance)
            {
                DesiredLocalAxis = Vector2.zero;
                return;
            }

            var worldDir = agent.desiredVelocity;
            worldDir.y = 0f;

            if (worldDir.sqrMagnitude <= Mathf.Epsilon)
            {
                DesiredLocalAxis = Vector2.zero;
                return;
            }

            var local = unit.Root.InverseTransformDirection(worldDir.normalized);
            DesiredLocalAxis = new Vector2(local.x, local.z);
        }
    }
}
