namespace Anchorpoint.Core.Units.Base
{
    /// <summary>
    /// Component that ticks every frame.
    /// </summary>
    public interface IUnitTick
    {
        /// <summary>
        /// Runs once per frame.
        /// </summary>
        void Tick(float dt);
    }
}
