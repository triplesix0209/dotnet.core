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
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Dữ liệu sau khi chuyển đổi.</returns>
        Task<TModel> ConvertEntityToModel(TEntity entity, CancellationToken cancellationToken = default);
    }
}
