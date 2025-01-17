namespace Sample.Application.Services
{
    public interface IAccountService : IStrongService<Account>,
        IStrongServiceRead<Account, AccountFilterAdminDto>,
        IStrongServiceCreate<Account, AccountCreateAdminDto>,
        IStrongServiceUpdate<Account, AccountUpdateAdminDto>
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
