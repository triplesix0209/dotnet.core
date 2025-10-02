using TripleSix.Core.Entities;

namespace TripleSix.Core.Types
{
    /// <summary>
    /// DTO có thể chuyển đổi thành Entity.
    /// </summary>
    /// <typeparam name="TEntity">Loại entity.</typeparam>
    public interface IMapToEntityDto<TEntity> : IDto
        where TEntity : IEntity
    {
        /// <summary>
        /// Chuyển đổi dữ liệu thành entity.
        /// </summary>
        /// <param name="serviceProvider"><see cref="IServiceProvider"/>.</param>
        /// <param name="source">Entity nguồn, dùng cho hành động update.</param>
        /// <returns>Entity đã được chuyển đổi từ Dto.</returns>
        Task<TEntity> ToEntity(IServiceProvider serviceProvider, TEntity? source = default);
    }
}
