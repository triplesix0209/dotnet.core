using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using TripleSix.Core.DataContext;

namespace TripleSix.Core.Helpers
{
    /// <summary>
    /// Helper xử lý Service.
    /// </summary>
    public static class ServiceHelper
    {
        /// <summary>
        /// Lấy Data context.
        /// </summary>
        /// <param name="serviceScope"><see cref="IServiceScope"/>.</param>
        /// <typeparam name="TDbContext">Type Data context cần resolve.</typeparam>
        /// <returns>Data context.</returns>
        public static TDbContext ResolveDataContext<TDbContext>(this IServiceScope serviceScope)
            where TDbContext : notnull, IDbDataContext
        {
            return serviceScope.ServiceProvider.GetAutofacRoot().Resolve<TDbContext>();
        }
    }
}