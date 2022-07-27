using System.Reflection;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TripleSix.Core.Entities;

namespace TripleSix.Core.Persistences
{
    /// <summary>
    /// DbContext cơ bản.
    /// </summary>
    public abstract class BaseDbContext : DbContext,
        IDbDataContext, IDbMigrationContext
    {
        private readonly Assembly _entityAssembly;
        private readonly Assembly _seedAssembly;

        /// <summary>
        /// Khởi tạo đối tượng BaseDbContext.
        /// </summary>
        /// <param name="entityAssembly">Assembly chứa các config của Entity.</param>
        /// <param name="seedAssembly">Assembly chứa các data seed.</param>
        protected BaseDbContext(Assembly entityAssembly, Assembly seedAssembly)
            : base()
        {
            _entityAssembly = entityAssembly;
            _seedAssembly = seedAssembly;
        }

        /// <inheritdoc/>
        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return Database.BeginTransactionAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task MigrateAsync(CancellationToken cancellationToken = default)
        {
            await Database.MigrateAsync(cancellationToken);
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

            modelBuilder.ApplyConfigurationsFromAssembly(_entityAssembly);

            var dataSeedTypes = _seedAssembly.GetExportedTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsAssignableTo<IDataSeed>());
            foreach (var dataSeedType in dataSeedTypes)
            {
                var dataSeed = Activator.CreateInstance(dataSeedType) as IDataSeed;
                dataSeed!.OnDataSeeding(modelBuilder);
            }
        }
    }
}
