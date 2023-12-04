using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TripleSix.CoreOld.Entities
{
    public abstract class BaseEntity<TEntity>
        : IEntity, IEntityTypeConfiguration<TEntity>
        where TEntity : class, IEntity, new()
    {
        public virtual object Clone()
        {
            var type = typeof(TEntity);

            var result = new TEntity();
            foreach (var p in type.GetProperties())
            {
                type.GetProperty(p.Name)?.SetValue(result, p.GetValue(this));
            }

            return result;
        }

        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            ModelConfigure(builder);
            SeedConfigure(builder);
        }

        protected virtual void ModelConfigure(EntityTypeBuilder<TEntity> builder)
        {
        }

        protected virtual void SeedConfigure(EntityTypeBuilder<TEntity> builder)
        {
        }
    }
}