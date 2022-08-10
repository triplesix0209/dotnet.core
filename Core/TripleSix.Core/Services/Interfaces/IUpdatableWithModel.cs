using TripleSix.Core.Entities;
using TripleSix.Core.Types;

namespace TripleSix.Core.Services
{
    public interface IUpdatableWithModel<TEntity, TModel> : IService<TEntity>
        where TEntity : class, IEntity
        where TModel : class, IDto
    {
        /// <summary>
        /// Cập nhật với dữ liệu đầu vào.
        /// </summary>
        /// <param name="entity">Entity sử dụng để cập nhận.</param>
        /// <param name="input">Data dùng để cập nhật entity.</param>
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Task xử lý.</returns>
        Task UpdateWithModel(TEntity entity, TModel input, CancellationToken cancellationToken = default);
    }
}
