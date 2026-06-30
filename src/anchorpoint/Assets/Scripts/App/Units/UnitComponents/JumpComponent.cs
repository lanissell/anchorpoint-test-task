using Anchorpoint.App.Units.UnitComponents.Views;
using Anchorpoint.Core.Units;
using Anchorpoint.Core.Units.Base;
using UnityEngine;

namespace Anchorpoint.App.Units.UnitComponents
{
    /// <summary>
    /// Makes the unit jump when grounded.
    /// </summary>
    public sealed class JumpComponent : IUnitComponent
    {
        private readonly float jumpHeight;

        private Unit unit;
        private JumpComponentView view;

        /// <summary>
        /// Creates the component with a jump height.
        /// </summary>
        public JumpComponent(float jumpHeight)
        {
            this.jumpHeight = jumpHeight;
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
        /// Jumps if on the ground.
        /// </summary>
        public void Jump()
        {
            if (!unit.TryGet(out GravityComponent gravity) || !gravity.IsGrounded)
            {
                return;
            }

            gravity.ApplyJump(Mathf.Sqrt(jumpHeight * -2f * gravity.Gravity));
            view?.Play();
        }
    }
}
