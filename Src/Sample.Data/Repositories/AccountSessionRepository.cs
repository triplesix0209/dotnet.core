using Sample.Data.DataContexts;
using Sample.Data.Entities;
using TripleSix.CoreOld.Repositories;

namespace Sample.Data.Repositories
{
    public class AccountSessionRepository : ModelRepository<AccountSessionEntity>
    {
        public AccountSessionRepository(DataContext context)
            : base(context)
        {
        }
    }
}
