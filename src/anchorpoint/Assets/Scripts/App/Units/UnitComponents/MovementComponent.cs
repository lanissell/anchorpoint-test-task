using Anchorpoint.App.Units.UnitComponents.Views;
using Anchorpoint.Core.Units;
using Anchorpoint.Core.Units.Base;
using UnityEngine;

namespace Anchorpoint.App.Units.UnitComponents
{
    /// <summary>
    /// Moves the unit on the ground at walk or sprint speed.
    /// </summary>
    public sealed class MovementComponent : IUnitComponent
    {
        private readonly float walkSpeed;
        private readonly float sprintSpeed;

        public float CurrentSpeed { get; private set; }

        private Unit unit;
        private MovementComponentView view;
        private float stepTimer;

        /// <summary>
        /// Creates the component with walk and sprint speeds.
        /// </summary>
        public MovementComponent(float walkSpeed, float sprintSpeed)
        {
            this.walkSpeed = walkSpeed;
            this.sprintSpeed = sprintSpeed;
        }

        /// <inheritdoc/>
        public void Attach(Unit attachedUnit)
        {
            unit = attachedUnit;
            unit.TryGet(out view);
        }

        /// <inheritdoc/>
        public void Detach()
        {
            unit = null;
        }

        /// <inheritdoc/>
        public void Reset() { }

        /// <summary>
        /// Moves the unit this frame (localAxis: x = strafe, y = forward).
        /// </summary>
        public void Move(Vector2 localAxis, bool sprint)
        {
            CurrentSpeed = sprint ? sprintSpeed : walkSpeed;

            if (unit.CharacterController.enabled)
            {
                var move = unit.Root.right * localAxis.x + unit.Root.forward * localAxis.y;
                unit.CharacterController.Move(move * CurrentSpeed * Time.deltaTime);
            }

            bool isGrounded = true;

            if (unit.TryGet(out GravityComponent gravity))
            {
                isGrounded = gravity.IsGrounded;
            }

            bool moving = localAxis != Vector2.zero && isGrounded;
            view?.SetLocomotion(moving, moving && sprint);
        }
    }
}
