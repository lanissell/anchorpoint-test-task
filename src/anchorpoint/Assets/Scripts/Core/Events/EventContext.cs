namespace Anchorpoint.Core.Events
{
    /// <summary>
    /// Where the player is, deciding which events can fire.
    /// </summary>
    public enum EventContext
    {
        /// <summary>
        /// On the base.
        /// </summary>
        OnBase,

        /// <summary>
        /// Out exploring.
        /// </summary>
        Exploring
    }
}
