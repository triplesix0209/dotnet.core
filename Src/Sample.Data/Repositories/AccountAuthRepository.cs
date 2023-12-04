using Sample.Data.DataContexts;
using Sample.Data.Entities;
using TripleSix.CoreOld.Repositories;

namespace Sample.Data.Repositories
{
    public class AccountAuthRepository : ModelRepository<AccountAuthEntity>
    {
        public AccountAuthRepository(DataContext context)
            : base(context)
        {
        }
    }
}
