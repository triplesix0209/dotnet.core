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
    public class AccountRepository : ModelRepository<AccountEntity>,
        IQueryBuilder<AccountEntity, AccountAdminDto.Filter>
    {
        public AccountRepository(DataContext context)
            : base(context)
        {
        }

        public async Task<IQueryable<AccountEntity>> BuildQuery(IIdentity identity, AccountAdminDto.Filter filter)
        {
            var query = await BuildQuery(identity, filter as ModelFilterDto);

            if (filter.Search.IsNotNullOrWhiteSpace())
            {
                query = query.WhereOrs(
                x => EF.Functions.Like(x.Code, $"%{filter.Search}%"),
                x => EF.Functions.Like(x.Email, $"%{filter.Search}%"),
                x => EF.Functions.Like(x.Name, $"%{filter.Search}%"),
                x => x.Auths.Any(y => y.Type == Common.Enum.AccountAuthTypes.UsernamePassword && EF.Functions.Like(y.Username, $"%{filter.Search}%")));
            }

            return query;
        }
    }
}
