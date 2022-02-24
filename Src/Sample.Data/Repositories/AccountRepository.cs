using Sample.Data.DataContexts;
using Sample.Data.Entities;
using TripleSix.Core.Repositories;

namespace Sample.Data.Repositories
{
    public class AccountRepository : ModelRepository<AccountEntity>
    {
        public AccountRepository(DataContext context)
            : base(context)
        {
        }
    }
}
