using TripleSix.Core.Entities;
using TripleSix.Core.Types;

namespace TripleSix.Core.Services
{
    public interface IReadableWithModel<TEntity, TModel> : IService<TEntity>
        where TEntity : class, IEntity
        where TModel : class, IDto
    {
        Task<TModel> ConvertEntityToModel(TEntity entity, CancellationToken cancellationToken = default);
    }
}
