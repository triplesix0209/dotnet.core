using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sample.Common.Dto;
using Sample.Data.DataContexts;
using Sample.Data.Entities;
using TripleSix.Core.Dto;
using TripleSix.Core.Helpers;
using TripleSix.Core.Repositories;

namespace Sample.Data.Repositories
{
    public class SettingRepository : ModelRepository<SettingEntity>,
        IQueryBuilder<SettingEntity, SettingFilterDto>
    {
        public SettingRepository(DataContext context)
            : base(context)
        {
        }

        public Task<IQueryable<SettingEntity>> BuildQuery(IIdentity identity, SettingFilterDto filter)
        {
            var query = BuildQuery()
                .WhereNotDeleted();

            if (filter.Code.IsNotNullOrWhiteSpace())
                query = query.Where(x => EF.Functions.Like(x.Code, $"%{filter.Code}%"));

            return Task.FromResult(query);
        }
    }
}
