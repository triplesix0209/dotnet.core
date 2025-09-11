using TripleSix.Core.Entities;
using TripleSix.Core.Types;

namespace TripleSix.Core.Services
{
    /// <summary>
    /// Service tạo dữ liệu.
    /// </summary>
    /// <typeparam name="TEntity">Loại entity xử lý.</typeparam>
    /// <typeparam name="TDto">Loại dto xử lý.</typeparam>
    public interface IStrongServiceCreate<TEntity, TDto> : IStrongService<TEntity>
        where TEntity : class, IStrongEntity
        where TDto : class, IMapToEntityDto<TEntity>
    {
        /// <summary>
        /// Khởi tạo dữ liệu với dto chỉ định.
        /// </summary>
        /// <param name="input">Dto sử dụng để ghi nhận.</param>
        /// <returns>Dữ liệu đã được tạo.</returns>
        Task<TEntity> Create(TDto input) => CreateWithMapper(input);
    }
}
