using System.Threading.Tasks;
using CpTech.Core.Dto;
using CpTech.Core.Entities;

namespace CpTech.Core.Services
{
    public interface IReadableWithModel<TEntity, TModel> :
        IModelService<TEntity>
        where TEntity : class, IModelEntity
        where TModel : class, IDto
    {
        Task<TModel> ConvertEntityToModel(IIdentity identity, TEntity entity, TModel model = null);
    }
}
