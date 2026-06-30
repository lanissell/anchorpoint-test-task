using Anchorpoint.Core.Units;
using Anchorpoint.Core.Units.Base;
using UnityEngine;

namespace Anchorpoint.App.Units.UnitComponents
{
    /// <summary>
    /// Pulls the unit down each frame and handles jumps.
    /// </summary>
    public sealed class GravityComponent : IUnitComponent, IUnitTick
    {
        /// <summary>
        /// Downward acceleration (negative).
        /// </summary>
        public float Gravity { get; }

        /// <summary>
        /// True if the unit is on the ground.
        /// </summary>
        public bool IsGrounded { get; private set; }

        private float velocityY;
        private CharacterController cc;

        /// <summary>
        /// Creates the component with the given gravity.
        /// </summary>
        public GravityComponent(float gravity)
        {
            Gravity = gravity;
        }

        /// <inheritdoc/>
        public void Attach(Unit unit)
        {
            cc = unit.CharacterController;
            IsGrounded = cc.isGrounded;
        }

        /// <inheritdoc/>
        public void Detach()
        {
            cc = null;
        }

        /// <inheritdoc/>
        public void Reset()
        {
            velocityY = 0f;
            IsGrounded = false;
        }

        /// <summary>
        /// Launches the unit upward.
        /// </summary>
        public void ApplyJump(float jumpVelocity)
        {
            velocityY = jumpVelocity;
        }

        /// <inheritdoc/>
        public void Tick(float dt)
        {
            if (!cc.enabled)
            {
                return;
            }

            if (IsGrounded && velocityY < 0f)
            {
                velocityY = -1.0f;
            }

            velocityY += Gravity * dt;
            cc.Move(new Vector3(0f, velocityY, 0f) * dt);
            IsGrounded = cc.isGrounded;
        }
    }
}
