using Anchorpoint.Core.Units;
using Anchorpoint.Core.Units.Base;
using UnityEngine;

namespace Anchorpoint.App.Units.UnitComponents
{
    /// <summary>
    /// Turns the body and tilts the camera.
    /// </summary>
    public sealed class LookComponent : IUnitComponent
    {
        private readonly float verticalClamp;

        public float TurnSpeed { get; private set; }

        private Transform root;
        private Transform cameraHolder;
        private float xRotation;

        /// <summary>
        /// Creates the component with a pitch limit and turn speed.
        /// </summary>
        public LookComponent(float verticalClamp, float turnSpeed)
        {
            this.verticalClamp = verticalClamp;
            TurnSpeed = turnSpeed;
        }

        /// <inheritdoc/>
        public void Attach(Unit unit)
        {
            root = unit.Root;
            cameraHolder = unit.GazeHolder;
        }

        /// <inheritdoc/>
        public void Detach()
        {
            cameraHolder = null;
            root = null;
        }

        /// <inheritdoc/>
        public void Reset()
        {
            xRotation = 0f;
        }

        /// <summary>
        /// Turns by a look delta this frame (x = yaw, y = pitch).
        /// </summary>
        public void Look(Vector2 delta)
        {
            xRotation -= delta.y * TurnSpeed;
            xRotation = Mathf.Clamp(xRotation, -verticalClamp, verticalClamp);

            cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            root.Rotate(Vector3.up * delta.x * TurnSpeed);
        }

        /// <summary>
        /// Slowly turns the body to face a world point.
        /// </summary>
        public void LookAt(Vector3 worldPoint, float dt)
        {
            Vector3 flat = worldPoint - root.position;
            flat.y = 0f;

            if (flat.sqrMagnitude < Mathf.Epsilon)
            {
                return;
            }

            Quaternion target = Quaternion.LookRotation(flat);
            root.rotation = Quaternion.RotateTowards(root.rotation, target, TurnSpeed * dt);
        }
    }
}
