using Sample.Data.DataContexts;
using Sample.Data.Entities;
using TripleSix.Core.Repositories;

namespace Sample.Data.Repositories
{
    public class AccountVerifyRepository : ModelRepository<AccountVerifyEntity>
    {
        public AccountVerifyRepository(DataContext context)
            : base(context)
        {
        }
    }
}
