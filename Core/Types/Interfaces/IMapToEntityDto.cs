using AutoMapper;
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
        /// <param name="mapper"><see cref="IMapper"/>.</param>
        /// <param name="serviceProvider"><see cref="IServiceProvider"/>.</param>
        /// <returns>Entity đã được chuyển đổi từ Dto.</returns>
        Task<TEntity> MapToEntity(IMapper mapper, IServiceProvider serviceProvider);

        /// <summary>
        /// Chuyển đổi dữ liệu thành entity, chỉ map các trường property có thay đổi so với entity gốc.
        /// </summary>
        /// <param name="mapper"><see cref="IMapper"/>.</param>
        /// <param name="serviceProvider"><see cref="IServiceProvider"/>.</param>
        /// <param name="sourceEntity">Entity gốc.</param>
        /// <returns>Entity đã được chuyển đổi từ Dto.</returns>
        Task<TEntity> MapChangeEntity(IMapper mapper, IServiceProvider serviceProvider, TEntity sourceEntity);
    }
}
