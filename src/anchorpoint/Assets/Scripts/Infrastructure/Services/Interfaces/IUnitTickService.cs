using Anchorpoint.Core.Units;
using System.Collections.Generic;

namespace Anchorpoint.Infrastructure.Services.Interfaces
{
    /// <summary>
    /// Tracks and ticks all live units.
    /// </summary>
    public interface IUnitTickService
    {
        /// <summary>
        /// All tracked units.
        /// </summary>
        IReadOnlyList<Unit> Units { get; }

        /// <summary>
        /// Starts ticking a unit.
        /// </summary>
        void Register(Unit unit);

        /// <summary>
        /// Removes and disposes a unit.
        /// </summary>
        void Unregister(Unit unit);

        /// <summary>
        /// Ticks all units each frame.
        /// </summary>
        void Tick(float dt);
    }
}
