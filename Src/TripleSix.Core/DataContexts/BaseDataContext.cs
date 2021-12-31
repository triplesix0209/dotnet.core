using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using TripleSix.Core.DataTypes;
using TripleSix.Core.Entities;

namespace TripleSix.Core.DataContexts
{
    public abstract class BaseDataContext : DbContext
    {
        private readonly Assembly _executingAssembly;

        protected BaseDataContext(Assembly executingAssembly, IConfiguration configuration)
        {
            _executingAssembly = executingAssembly;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public bool IsMigration { get; set; }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            var addedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Added);
            foreach (var entity in addedEntities)
            {
                var createDatetime = entity.Properties.FirstOrDefault(x => x.Metadata.Name == nameof(IModelEntity.CreateDatetime));
                if (createDatetime != null && createDatetime.CurrentValue == null) createDatetime.CurrentValue = now;

                var updateDatetime = entity.Properties.FirstOrDefault(x => x.Metadata.Name == nameof(IModelEntity.UpdateDatetime));
                if (updateDatetime != null && updateDatetime.CurrentValue == null) updateDatetime.CurrentValue = now;
            }

            var modifiedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified);
            foreach (var entity in modifiedEntities)
            {
                var updateDatetime = entity.Properties.FirstOrDefault(x => x.Metadata.Name == nameof(IModelEntity.UpdateDatetime));
                if (updateDatetime != null) updateDatetime.CurrentValue = now;
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);

            builder.EnableSensitiveDataLogging()
                .UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(_executingAssembly);

            foreach (var entity in builder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    if (property.PropertyInfo.CustomAttributes.Any(x => x.AttributeType == typeof(PhoneAttribute)))
                    {
                        property.SetColumnType("nvarchar(20)");
                        property.SetValueConverter(new ValueConverter<string, string>(
                            v => new Phone(v).ToString(),
                            v => new Phone(v).ToString()));
                    }
                }
            }
        }
    }
}