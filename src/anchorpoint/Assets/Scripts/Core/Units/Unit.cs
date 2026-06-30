using Anchorpoint.Core.Units.Base;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Anchorpoint.Core.Units
{
    /// <summary>
    /// A unit built from components that get ticked each frame.
    /// </summary>
    public sealed class Unit : IDisposable
    {
        /// <summary>
        /// Point the unit looks from.
        /// </summary>
        public Transform GazeHolder { get; }

        /// <summary>
        /// The unit's character controller.
        /// </summary>
        public CharacterController CharacterController { get; }

        /// <summary>
        /// The unit's root transform.
        /// </summary>
        public Transform Root => CharacterController.transform;

        private readonly Dictionary<Type, IUnitComponent> components = new Dictionary<Type, IUnitComponent>();

        private readonly List<IUnitTick> tickables = new List<IUnitTick>();

        /// <summary>
        /// Creates a unit from a controller and gaze holder.
        /// </summary>
        public Unit(CharacterController characterController, Transform gazeHolder)
        {
            CharacterController = characterController;
            GazeHolder = gazeHolder;
        }

        /// <summary>
        /// Adds a component and attaches it.
        /// </summary>
        public T Add<T>(T component) where T : IUnitComponent
        {
            var type = component.GetType();
            components[type] = component;

            if (component is IUnitTick tick)
            {
                tickables.Add(tick);
            }

            component.Attach(this);
            return component;
        }

        /// <summary>
        /// Adds prebuilt scene components (like views) before the config-built ones.
        /// </summary>
        public void AddSceneComponents(IReadOnlyList<IUnitComponent> sceneComponents)
        {
            if (sceneComponents == null)
            {
                return;
            }

            for (int i = 0; i < sceneComponents.Count; i++)
            {
                if (sceneComponents[i] != null)
                {
                    Add(sceneComponents[i]);
                }
            }
        }

        /// <summary>
        /// Gets the component of type T if it exists.
        /// </summary>
        public bool TryGet<T>(out T result) where T : IUnitComponent
        {
            if (components.TryGetValue(typeof(T), out IUnitComponent component))
            {
                result = (T)component;
                return true;
            }

            result = default;
            return false;
        }

        /// <summary>
        /// Ticks every component once per frame.
        /// </summary>
        public void Tick(float dt)
        {
            for (int i = 0; i < tickables.Count; i++)
            {
                tickables[i].Tick(dt);
            }
        }

        /// <summary>
        /// Resets every component before the unit goes back to the pool.
        /// </summary>
        public void Reset()
        {
            foreach (var component in components.Values)
            {
                component.Reset();
            }
        }

        /// <summary>
        /// Detaches every component and clears all state.
        /// </summary>
        public void Dispose()
        {
            foreach (var component in components.Values)
            {
                component.Detach();
            }

            components.Clear();
            tickables.Clear();
        }
    }
}
