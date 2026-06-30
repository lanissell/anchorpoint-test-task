namespace Anchorpoint.Core.Interaction
{
    /// <summary>
    /// Something the player can pick up.
    /// </summary>
    public interface IPickup
    {
        /// <summary>
        /// Collects this pickup.
        /// </summary>
        void Collect();
    }
}
