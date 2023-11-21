using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TripleSix.Core.Entities
{
    /// <summary>
    /// Entity cơ bản.
    /// </summary>
    /// <typeparam name="TEntity">Kiểu Entity, khai báo lại để sử dụng cho IEntityTypeConfiguration.</typeparam>
    public abstract class BaseEntity<TEntity>
        : IEntity, IEntityTypeConfiguration<TEntity>
        where TEntity : class, IEntity
    {
        /// <inheritdoc/>
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
        }
    }
}
