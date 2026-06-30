using Anchorpoint.Infrastructure.Services.Base;
using Infrastructure;
using System;

namespace Anchorpoint.Infrastructure.Services.Interfaces
{
    /// <summary>
    /// Gives access to the player's input actions.
    /// </summary>
    public interface IInputActionsProviderService : IDisposable
    {
        /// <summary>
        /// The active player input map.
        /// </summary>
        InputSystem_Actions.PlayerActions PlayerActions { get; }
    }
}
