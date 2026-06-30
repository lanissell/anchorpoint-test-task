namespace Anchorpoint.Core.Units.Base
{
    /// <summary>
    /// Base contract for every unit component.
    /// </summary>
    public interface IUnitComponent
    {
        /// <summary>
        /// Called when the component is added to a unit.
        /// </summary>
        void Attach(Unit unit);

        /// <summary>
        /// Called when the component is removed.
        /// </summary>
        void Detach();

        /// <summary>
        /// Resets the component before the unit is pooled.
        /// </summary>
        void Reset();
    }
}
