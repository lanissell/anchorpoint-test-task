using Anchorpoint.Core.Units;
using Anchorpoint.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Anchorpoint.Infrastructure.Services.Implementations
{
    /// <summary>
    /// Tracks and ticks all live units.
    /// </summary>
    public sealed class UnitTickService : IUnitTickService, IDisposable
    {
        private readonly List<Unit> units = new List<Unit>();

        /// <inheritdoc/>
        public IReadOnlyList<Unit> Units => units;

        /// <inheritdoc/>
        public void Register(Unit unit)
        {
            units.Add(unit);
        }

        /// <inheritdoc/>
        public void Unregister(Unit unit)
        {
            if (units.Remove(unit))
            {
                unit.Dispose();
            }
        }

        /// <inheritdoc/>
        public void Tick(float dt)
        {
            for (int i = 0; i < units.Count; i++)
            {
                units[i].Tick(dt);
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            for (int i = units.Count - 1; i >= 0; i--)
            {
                units[i].Dispose();
            }

            units.Clear();
        }
    }
}
