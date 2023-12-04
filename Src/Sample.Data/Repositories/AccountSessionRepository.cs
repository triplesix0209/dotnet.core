using Sample.Data.DataContexts;
using Sample.Data.Entities;

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
