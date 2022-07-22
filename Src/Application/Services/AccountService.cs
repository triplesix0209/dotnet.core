namespace Sample.Application.Services
{
    public class AccountService : StrongService<Account>, IAccountService
    {
        public AccountService(IApplicationDbContext db)
            : base(db)
        {
        }
    }
}
