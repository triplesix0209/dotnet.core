using System.Linq;
using System.Threading.Tasks;
using TripleSix.CoreOld.Dto;
using TripleSix.CoreOld.Entities;

namespace TripleSix.CoreOld.Repositories
{
    public interface IQueryBuilder<TEntity, TFilterDto>
        where TEntity : IEntity
        where TFilterDto : IFilterDto
    {
        Task<IQueryable<TEntity>> BuildQuery(IIdentity identity, TFilterDto filter);
    }
}