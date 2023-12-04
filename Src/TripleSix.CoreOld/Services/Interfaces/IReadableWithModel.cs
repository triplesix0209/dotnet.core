using System.Threading.Tasks;
using TripleSix.CoreOld.Dto;
using TripleSix.CoreOld.Entities;

namespace TripleSix.CoreOld.Services
{
    public interface IReadableWithModel<TEntity, TModel> :
        IModelService<TEntity>
        where TEntity : class, IModelEntity
        where TModel : class, IDto
    {
        Task<TModel> ConvertEntityToModel(IIdentity identity, TEntity entity, TModel model = null);
    }
}
