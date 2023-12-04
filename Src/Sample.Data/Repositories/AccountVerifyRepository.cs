using Sample.Data.DataContexts;
using Sample.Data.Entities;

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
