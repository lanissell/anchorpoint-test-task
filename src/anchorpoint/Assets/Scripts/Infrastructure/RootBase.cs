using Anchorpoint.Infrastructure.Services.Base;
using System;
using System.Threading;
using UnityEngine;

namespace Anchorpoint.Infrastructure
{
    /// <summary>
    /// Base for scene roots; sets up a cancellation token and calls Initialize on Awake.
    /// </summary>
    [DefaultExecutionOrder(Constants.RootExecutionOrder)]
    public abstract class RootBase : MonoBehaviour, IDisposable
    {
        private CancellationTokenSource rootCts;
        private bool disposed;

        /// <summary>
        /// Sets up the token and initializes the root.
        /// </summary>
        protected virtual void Awake()
        {
            rootCts = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken);

            var serviceResolver = AppLoader.ServiceResolver;
            Initialize(serviceResolver, rootCts.Token);
        }

        /// <summary>
        /// Disposes the root when destroyed.
        /// </summary>
        protected virtual void OnDestroy()
        {
            Dispose();
        }

        /// <summary>
        /// Sets up the root.
        /// </summary>
        protected abstract void Initialize(IServiceResolver serviceResolver, CancellationToken rootCt);

        /// <summary>
        /// Tears down the root and cancels its token.
        /// </summary>
        protected virtual void DeInitialize()
        {
            rootCts?.Cancel();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            disposed = true;

            DeInitialize();
        }
    }
}
