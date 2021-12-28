using System.Threading.Tasks;
using TripleSix.Core.Dto;
using TripleSix.Core.Entities;

namespace TripleSix.Core.Services
{
    public interface IReadableWithModel<TEntity, TModel> :
        IModelService<TEntity>
        where TEntity : class, IModelEntity
        where TModel : class, IDto
    {
        Task<TModel> ConvertEntityToModel(IIdentity identity, TEntity entity, TModel model = null);
    }
}
