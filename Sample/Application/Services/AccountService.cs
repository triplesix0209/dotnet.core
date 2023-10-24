namespace Sample.Application.Services
{
    public interface IAccountService : IStrongService<Account>
    {
    }

    public class AccountService : StrongService<Account>, IAccountService
    {
        public AccountService(IDbDataContext db)
            : base(db)
        {
        }

        public IApplicationDbContext Db { get; set; }
    }
}
