using TripleSix.Core.Entities;
using TripleSix.Core.Types;

namespace TripleSix.Core.Services
{
    public interface IReadableWithModel<TEntity, TModel> : IService<TEntity>
        where TEntity : class, IEntity
        where TModel : class, IDto
    {
        /// <summary>
        /// Chuyển đổi dữ liệu entity sang kiểu chỉ định.
        /// </summary>
        /// <param name="entity">entity sẽ chuyển đổi.</param>
        /// <param name="result">kết quả chuyển đổi.</param>
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns><see cref="Task"/>.</returns>
        Task ConvertEntityToModel(TEntity entity, out TModel result, CancellationToken cancellationToken = default);
    }
}
