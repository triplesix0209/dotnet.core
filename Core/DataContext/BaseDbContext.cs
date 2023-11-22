using System.Data;
using System.Reflection;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TripleSix.Core.Entities;
using TripleSix.Core.Identity;

namespace TripleSix.Core.DataContext
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

        /// <summary>
        /// Identity context.
        /// </summary>
        public IIdentityContext Identity { get; set; }

        /// <inheritdoc/>
        public IDbContextTransaction? CurrentTransaction => Database.CurrentTransaction;

        /// <inheritdoc/>
        public IDbContextTransaction BeginTransaction()
        {
            return Database.BeginTransaction();
        }

        /// <inheritdoc/>
        public IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return Database.BeginTransaction(isolationLevel);
        }

        /// <inheritdoc/>
        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await Database.BeginTransactionAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default)
        {
            return await Database.BeginTransactionAsync(isolationLevel, cancellationToken);
        }

        /// <inheritdoc/>
        public void Migrate()
        {
            Database.Migrate();
        }

        /// <inheritdoc/>
        public async Task MigrateAsync(CancellationToken cancellationToken = default)
        {
            await Database.MigrateAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public new virtual int SaveChanges(bool autoAudit = true)
        {
            if (autoAudit)
            {
                var now = DateTime.UtcNow;

                var addedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Added);
                foreach (var entity in addedEntities)
                {
                    var createAt = entity.Properties.FirstOrDefault(x => x.Metadata.Name == nameof(IStrongEntity.CreateAt));
                    if (createAt != null && createAt.CurrentValue == null) createAt.CurrentValue = now;

                    var creatorId = entity.Properties.FirstOrDefault(x => x.Metadata.Name == nameof(IStrongEntity.CreatorId));
                    if (creatorId != null && creatorId.CurrentValue == null && Identity.Id.HasValue) creatorId.CurrentValue = Identity.Id;

                    var updateAt = entity.Properties.FirstOrDefault(x => x.Metadata.Name == nameof(IStrongEntity.UpdateAt));
                    if (updateAt != null && updateAt.CurrentValue == null) updateAt.CurrentValue = now;

                    var updatorId = entity.Properties.FirstOrDefault(x => x.Metadata.Name == nameof(IStrongEntity.UpdatorId));
                    if (updatorId != null && updatorId.CurrentValue == null && Identity.Id.HasValue) updatorId.CurrentValue = Identity.Id;
                }

                var modifiedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified);
                foreach (var entity in modifiedEntities)
                {
                    var updateAt = entity.Properties.FirstOrDefault(x => x.Metadata.Name == nameof(IStrongEntity.UpdateAt));
                    if (updateAt != null) updateAt.CurrentValue = now;

                    var updatorId = entity.Properties.FirstOrDefault(x => x.Metadata.Name == nameof(IStrongEntity.UpdatorId));
                    if (updatorId != null && updatorId.CurrentValue == null && Identity.Id.HasValue) updatorId.CurrentValue = Identity.Id;
                }
            }

            return base.SaveChanges();
        }

        /// <inheritdoc/>
        public new virtual Task<int> SaveChangesAsync(bool autoAudit = true, CancellationToken cancellationToken = default)
        {
            if (autoAudit)
            {
                var now = DateTime.UtcNow;

                var addedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Added);
                foreach (var entity in addedEntities)
                {
                    var createAt = entity.Properties.FirstOrDefault(x => x.Metadata.Name == nameof(IStrongEntity.CreateAt));
                    if (createAt != null && createAt.CurrentValue == null) createAt.CurrentValue = now;

                    var creatorId = entity.Properties.FirstOrDefault(x => x.Metadata.Name == nameof(IStrongEntity.CreatorId));
                    if (creatorId != null && creatorId.CurrentValue == null && Identity.Id.HasValue) creatorId.CurrentValue = Identity.Id;

                    var updateAt = entity.Properties.FirstOrDefault(x => x.Metadata.Name == nameof(IStrongEntity.UpdateAt));
                    if (updateAt != null && updateAt.CurrentValue == null) updateAt.CurrentValue = now;

                    var updatorId = entity.Properties.FirstOrDefault(x => x.Metadata.Name == nameof(IStrongEntity.UpdatorId));
                    if (updatorId != null && updatorId.CurrentValue == null && Identity.Id.HasValue) updatorId.CurrentValue = Identity.Id;
                }

                var modifiedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified);
                foreach (var entity in modifiedEntities)
                {
                    var updateAt = entity.Properties.FirstOrDefault(x => x.Metadata.Name == nameof(IStrongEntity.UpdateAt));
                    if (updateAt != null) updateAt.CurrentValue = now;

                    var updatorId = entity.Properties.FirstOrDefault(x => x.Metadata.Name == nameof(IStrongEntity.UpdatorId));
                    if (updatorId != null && updatorId.CurrentValue == null && Identity.Id.HasValue) updatorId.CurrentValue = Identity.Id;
                }
            }

            return SaveChangesAsync(cancellationToken);
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
                .Where(x => x.IsAssignableTo<IDbDataSeed>());
            foreach (var dataSeedType in dataSeedTypes)
            {
                var dataSeed = Activator.CreateInstance(dataSeedType) as IDbDataSeed;
                dataSeed!.OnDataSeeding(modelBuilder, Database);
            }
        }
    }
}
