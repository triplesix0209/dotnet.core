using TripleSix.Core.Entities;
using TripleSix.Core.Types;

namespace TripleSix.Core.Services
{
    /// <summary>
    /// Service cập nhật dữ liệu.
    /// </summary>
    /// <typeparam name="TEntity">Loại entity xử lý.</typeparam>
    /// <typeparam name="TDto">Loại dto xử lý.</typeparam>
    public interface IStrongServiceUpdate<TEntity, TDto> : IStrongService<TEntity>
        where TEntity : class, IStrongEntity
        where TDto : class, IDto
    {
        /// <summary>
        /// Cập nhật dữ liệu với dto chỉ định.
        /// </summary>
        /// <param name="entity">Entity sử dụng để cập nhật.</param>
        /// <param name="input">Dto sử dụng để ghi nhận.</param>
        /// <returns>Dữ liệu đã sau chỉnh sửa.</returns>
        Task<TEntity> Update(TEntity entity, TDto input) => UpdateWithMapper(entity, input);
    }
}
