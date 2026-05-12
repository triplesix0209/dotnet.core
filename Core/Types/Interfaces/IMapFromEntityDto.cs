using TripleSix.Core.Entities;

namespace TripleSix.Core.Types
{
    /// <summary>
    /// DTO có thể nạp từ Entity.
    /// </summary>
    /// <typeparam name="TEntity">Loại entity.</typeparam>
    public interface IMapFromEntityDto<TEntity> : IDto
        where TEntity : IEntity
    {
        /// <summary>
        /// Nạp dữ liệu từ entity.
        /// </summary>
        /// <param name="serviceProvider"><see cref="IServiceProvider"/>.</param>
        /// <param name="source">Entity sử dụng để chuyển đổi.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task FromEntity(IServiceProvider serviceProvider, TEntity source);
    }
}
