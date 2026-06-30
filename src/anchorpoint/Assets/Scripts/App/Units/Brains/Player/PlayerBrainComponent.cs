using Anchorpoint.App.Units.UnitComponents;
using Anchorpoint.Core.Units;
using Anchorpoint.Core.Units.Base;
using Anchorpoint.Infrastructure.Services.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Anchorpoint.App.Units.Brains.Player
{
    /// <summary>
    /// Reads player input each frame and drives the unit.
    /// </summary>
    public sealed class PlayerBrainComponent : IUnitComponent, IUnitTick
    {
        private Unit unit;
        private IInputActionsProviderService input;

        /// <inheritdoc/>
        public void Attach(Unit attachedUnit)
        {
            unit = attachedUnit;
            input = AppLoader.ServiceResolver.Resolve<IInputActionsProviderService>();

            input.PlayerActions.Interact.performed += OnInteractPerformed;
            input.PlayerActions.Meele.performed += OnMeleePerformed;
        }

        /// <inheritdoc/>
        public void Detach()
        {
            input.PlayerActions.Interact.performed -= OnInteractPerformed;
            input.PlayerActions.Meele.performed -= OnMeleePerformed;

            unit = null;
            input = null;
        }

        /// <inheritdoc/>
        public void Reset() { }

        /// <inheritdoc/>
        public void Tick(float dt)
        {
            var actions = input.PlayerActions;

            var moveAxis = actions.Move.ReadValue<Vector2>();
            bool sprint = actions.Sprint.IsPressed();

            if (unit.TryGet(out MovementComponent movement))
            {
                movement.Move(moveAxis, sprint);
            }

            var lookDelta = actions.Look.ReadValue<Vector2>();

            if (unit.TryGet(out LookComponent look))
            {
                look.Look(lookDelta);
            }

            if (actions.Jump.WasPressedThisFrame())
            {
                if (unit.TryGet(out JumpComponent jump))
                {
                    jump.Jump();
                }
            }

            if (unit.TryGet(out RangedAttackComponent ranged))
            {
                bool fire = ranged.IsAutomatic
                    ? actions.Attack.IsPressed()
                    : actions.Attack.WasPressedThisFrame();

                if (fire)
                {
                    ranged.Shoot();
                }
            }
        }

        private void OnInteractPerformed(InputAction.CallbackContext context)
        {
            if (unit.TryGet(out InteractorComponent interactor))
            {
                interactor.TryCollectNearest();
            }
        }

        private void OnMeleePerformed(InputAction.CallbackContext context)
        {
            if (unit.TryGet(out MeleeAttackComponent melee))
            {
                melee.Swing();
            }
        }
    }
}
