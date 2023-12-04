using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sample.Common.Dto;
using Sample.Data.DataContexts;
using Sample.Data.Entities;
using TripleSix.CoreOld.Dto;
using TripleSix.CoreOld.Helpers;
using TripleSix.CoreOld.Repositories;

namespace Sample.Data.Repositories
{
    public class TestRepository : ModelRepository<TestEntity>,
        IQueryBuilder<TestEntity, TestAdminDto.Filter>
    {
        public TestRepository(DataContext context)
            : base(context)
        {
        }

        public async Task<IQueryable<TestEntity>> BuildQuery(IIdentity identity, TestAdminDto.Filter filter)
        {
            var query = await BuildQuery(identity, filter as ModelFilterDto);

            if (filter.Search.IsNotNullOrWhiteSpace())
            {
                query = query.WhereOrs(
                x => EF.Functions.Like(x.Code, $"%{filter.Search}%"),
                x => EF.Functions.Like(x.Name, $"%{filter.Search}%"));
            }

            return query;
        }
    }
}
