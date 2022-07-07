using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TripleSix.Core.Persistences.Interfaces;

namespace TripleSix.Core.Persistences
{
    /// <summary>
    /// DbContext cơ bản.
    /// </summary>
    public abstract class BaseDbContext : DbContext, IDbDataContext, IDbMigrationContext
    {
        private readonly Assembly _assembly;

        /// <summary>
        /// Khởi tạo đối tượng BaseDbContext.
        /// </summary>
        /// <param name="assembly">Assembly được sử dụng để nạp config của các Entity.</param>
        protected BaseDbContext(Assembly assembly)
            : base()
        {
            _assembly = assembly;
        }

        /// <inheritdoc/>
        public Task MigrateAsync(CancellationToken cancellationToken = default)
        {
            return Database.MigrateAsync(cancellationToken);
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
