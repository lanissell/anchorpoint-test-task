namespace Anchorpoint.Infrastructure.Services.Base
{
    /// <summary>
    /// Resolves services.
    /// </summary>
    public interface IServiceResolver
    {
        /// <summary>
        /// Gets a service.
        /// </summary>
        T Resolve<T>();
    }
}