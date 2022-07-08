using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TripleSix.Core.Entities.Interfaces;
using TripleSix.Core.Persistences.Interfaces;

namespace TripleSix.Core.Persistences
{
    /// <summary>
    /// DbContext cơ bản.
    /// </summary>
    public abstract class BaseDbContext : DbContext,
        IDbDataContext, IDbMigrationContext
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
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            var addedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Added);
            foreach (var entity in addedEntities)
            {
                var createDateTime = entity.Properties
                    .FirstOrDefault(x => x.Metadata.Name == nameof(IStrongEntity.CreateDateTime));
                if (createDateTime != null && createDateTime.CurrentValue == null)
                    createDateTime.CurrentValue = now;

                var updateDateTime = entity.Properties
                    .FirstOrDefault(x => x.Metadata.Name == nameof(IStrongEntity.UpdateDateTime));
                if (updateDateTime != null && updateDateTime.CurrentValue == null)
                    updateDateTime.CurrentValue = now;
            }

            var modifiedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified);
            foreach (var entity in modifiedEntities)
            {
                var updateDateTime = entity.Properties
                    .FirstOrDefault(x => x.Metadata.Name == nameof(IStrongEntity.UpdateDateTime));
                if (updateDateTime != null && updateDateTime.CurrentValue == null)
                    updateDateTime.CurrentValue = now;
            }

            return base.SaveChangesAsync(cancellationToken);
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
