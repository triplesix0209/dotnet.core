using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TripleSix.Core.Interfaces.DbContext;

namespace TripleSix.Core.Persistences
{
    public abstract class BaseDbContext : DbContext, IApplicationDbContext
    {
        private readonly Assembly _assembly;

        /// <summary>
        /// Khởi tạo đối tượng BaseDbContext.
        /// </summary>
        /// <param name="assembly">Assembly được sử dụng để nạp config của các Entity.</param>
        /// <param name="configuration">Configuration (appsettings) chứa connection string đến database.</param>
        protected BaseDbContext(Assembly assembly)
            : base()
        {
            _assembly = assembly;
        }

        /// <inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseLazyLoadingProxies();
        }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(_assembly);
        }
    }
}
