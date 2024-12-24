using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TripleSix.Core.Entities;

namespace TripleSix.Core.DataContext
{
    /// <summary>
    /// Data Seeder phát sinh dữ liệu.
    /// </summary>
    /// <typeparam name="TEntity">Kiểu Entity, khai báo lại để sử dụng cho IEntityTypeConfiguration.</typeparam>
    public abstract class BaseDataSeed<TEntity> : IDbDataSeed
        where TEntity : class, IEntity
    {
        /// <inheritdoc/>
        public abstract void OnDataSeeding(ModelBuilder builder, DatabaseFacade database);
    }
}
