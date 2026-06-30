using Anchorpoint.App.Units.UnitComponents;
using Anchorpoint.Core.Units;
using Anchorpoint.Core.Units.Base;
using Anchorpoint.Infrastructure;
using Anchorpoint.Infrastructure.Services.Interfaces;
using UnityEngine;

namespace Anchorpoint.App.Units.Brains.Enemy
{
    /// <summary>
    /// Makes the enemy chase the player.
    /// </summary>
    public class EnemyBrainComponent : IUnitComponent, IUnitTick
    {
        private Unit unit;
        private IPlayerContextService playerContextService;

        /// <inheritdoc/>
        public void Attach(Unit unit)
        {
            this.unit = unit;
            playerContextService = AppLoader.ServiceResolver.Resolve<IPlayerContextService>();
        }

        /// <inheritdoc/>
        public void Detach()
        {
        }

        /// <inheritdoc/>
        public void Reset() { }

        /// <inheritdoc/>
        public void Tick(float dt)
        {
            Vector2 moveAxis = Vector2.zero;

            if (unit.TryGet(out HealthComponent health) && health.IsDead)
            {
                return;
            }

            if (unit.TryGet(out NavigationComponent navigation))
            {
                navigation.SetDestination(playerContextService.Player.position);
                moveAxis = navigation.DesiredLocalAxis;
            }

            if (unit.TryGet(out LookComponent look))
            {
                if (moveAxis.sqrMagnitude > Mathf.Epsilon)
                {
                    Vector3 worldDir = unit.Root.TransformDirection(new Vector3(moveAxis.x, 0f, moveAxis.y));
                    look.LookAt(unit.Root.position + worldDir, dt);
                }
                else
                {
                    look.LookAt(playerContextService.Player.position, dt);
                }
            }

            if (unit.TryGet(out MovementComponent movement))
            {
                movement.Move(moveAxis, true);
            }

            if (unit.TryGet(out MeleeAttackComponent melee))
            {
                melee.TryAttack();
            }
        }
    }
}
