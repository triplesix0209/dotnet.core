using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TripleSix.Core.Interfaces.Entity;

namespace TripleSix.Core.Persistences
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
        public abstract void Configure(EntityTypeBuilder<TEntity> builder);
    }
}
