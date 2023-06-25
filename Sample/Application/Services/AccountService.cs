namespace Sample.Application.Services
{
    public interface IAccountService : IStrongService<Account>
    {
        Task<IPaging<AccountDataDto>> GetPageActivate(int page = 1, int size = 10);
    }

    public class AccountService : StrongService<Account>, IAccountService
    {
        public AccountService(IDbDataContext db)
            : base(db)
        {
        }

        public IApplicationDbContext Db { get; set; }

        public async Task<IPaging<AccountDataDto>> GetPageActivate(int page = 1, int size = 10)
        {
            var query = Db.Account
                .WhereNotDeleted();

            return await GetPage<AccountDataDto>(query, page, size);
        }
    }
}
