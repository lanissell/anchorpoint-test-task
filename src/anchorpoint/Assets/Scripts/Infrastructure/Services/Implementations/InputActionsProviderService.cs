using Anchorpoint.Infrastructure.Services.Interfaces;
using Infrastructure;

namespace Anchorpoint.Infrastructure.Services.Implementations
{
    /// <summary>
    /// Gives access to the player's input actions.
    /// </summary>
    public sealed class InputActionsProviderService : IInputActionsProviderService
    {
        private InputSystem_Actions actions;

        /// <inheritdoc/>
        public InputSystem_Actions.PlayerActions PlayerActions { get; }

        private bool disposed;

        /// <summary>
        /// Creates and enables the player input map.
        /// </summary>
        public InputActionsProviderService()
        {
            actions = new InputSystem_Actions();

            PlayerActions = actions.Player;
            PlayerActions.Enable();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            disposed = true;

            PlayerActions.Disable();
            actions.Dispose();
        }
    }
}
