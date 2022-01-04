using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using TripleSix.Core.Extensions;

namespace TripleSix.Core.DataContexts
{
    public abstract class MySqlContext : BaseDataContext
    {
        protected MySqlContext(Assembly executingAssembly, IConfiguration configuration)
            : base(executingAssembly, configuration)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
            var serverVersion = new MySqlServerVersion(new Version(8, 0));
            builder.UseMySql(Configuration.GetConnectionString(IsMigration ? "Migration" : "Default"), serverVersion);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var entity in builder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.GetTableName().ToSnakeCase());

                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnBaseName().ToSnakeCase());

                    if (property.ClrType == typeof(Guid)
                        || Nullable.GetUnderlyingType(property.ClrType) == typeof(Guid))
                        property.SetValueConverter(new GuidToStringConverter());
                }

                foreach (var key in entity.GetKeys())
                    key.SetName(key.GetName().ToSnakeCase());

                foreach (var key in entity.GetForeignKeys())
                    key.SetConstraintName(key.GetConstraintName().ToSnakeCase());

                foreach (var index in entity.GetIndexes())
                    index.SetDatabaseName(index.GetDatabaseName().ToSnakeCase());
            }
        }
    }
}