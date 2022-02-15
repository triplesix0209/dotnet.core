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
        IQueryBuilder<SettingEntity, SettingAdminDto.Filter>,
        IQueryBuilder<SettingEntity, SettingFilterDto>
    {
        public SettingRepository(DataContext context)
            : base(context)
        {
        }

        public async Task<IQueryable<SettingEntity>> BuildQuery(IIdentity identity, SettingAdminDto.Filter filter)
        {
            var query = await BuildQuery(identity, filter as ModelFilterDto);

            if (filter.Search.IsNotNullOrWhiteSpace())
            {
                query = query.WhereOrs(
                    x => EF.Functions.Like(x.Code, $"%{filter.Search}%"),
                    x => EF.Functions.Like(x.Description, $"%{filter.Search}%"));
            }

            if (filter.Description.IsNotNullOrWhiteSpace())
                query = query.Where(x => EF.Functions.Like(x.Description, $"%{filter.Description}%"));

            return query;
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
