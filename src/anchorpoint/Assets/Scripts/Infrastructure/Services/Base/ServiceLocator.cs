using System;
using System.Collections.Generic;
using System.Linq;

namespace Anchorpoint.Infrastructure.Services.Base
{
    /// <summary>
    /// Stores and resolves services by interface type.
    /// </summary>
    public sealed class ServiceLocator : IServiceResolver
    {
        private readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

        /// <summary>
        /// Removes all services.
        /// </summary>
        public void DeInitialize()
        {
            while (services.Count > 0)
            {
                Unregister(services.Keys.First());
            }
        }

        /// <summary>
        /// Removes the service of type T.
        /// </summary>
        public void Unregister<T>()
        {
            Unregister(typeof(T));
        }

        /// <summary>
        /// Registers a service under its interface T.
        /// </summary>
        public void Register<T>(T service)
        {
            if (typeof(T).IsInterface)
            {
                services[typeof(T)] = service;
            }
        }

        /// <inheritdoc/>
        public T Resolve<T>()
        {
            if (services.TryGetValue(typeof(T), out object service) && service is T t)
            {
                return t;
            }

            throw new($"Service {typeof(T)} not found in Game Services.");
        }

        private void Unregister(Type type)
        {
            if (!services.Remove(type, out object service))
            {
                return;
            }

            if (service is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
