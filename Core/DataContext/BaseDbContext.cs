using System.Reflection;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

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

        public IDbContextTransaction BeginTransaction()
        {
            return Database.BeginTransaction();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await Database.BeginTransactionAsync(cancellationToken);
        }

        public void Migrate()
        {
            Database.Migrate();
        }

        public async Task MigrateAsync(CancellationToken cancellationToken = default)
        {
            await Database.MigrateAsync(cancellationToken);
        }

        public new virtual int SaveChanges(bool autoAudit = true)
        {
            if (!autoAudit) return base.SaveChanges();
            return base.SaveChanges();
        }

        public new virtual Task<int> SaveChangesAsync(bool autoAudit = true, CancellationToken cancellationToken = default)
        {
            if (!autoAudit) return SaveChangesAsync(cancellationToken);
            return SaveChangesAsync(cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseLazyLoadingProxies();
        }

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
                dataSeed!.OnDataSeeding(modelBuilder);
            }
        }
    }
}
