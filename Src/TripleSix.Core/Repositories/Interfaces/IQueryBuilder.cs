using System.Linq;
using System.Threading.Tasks;
using TripleSix.Core.Dto;
using TripleSix.Core.Entities;

namespace TripleSix.Core.Repositories
{
    public interface IQueryBuilder<TEntity, TFilterDto>
        where TEntity : IEntity
        where TFilterDto : IFilterDto
    {
        Task<IQueryable<TEntity>> BuildQuery(IIdentity identity, TFilterDto filter);
    }
}