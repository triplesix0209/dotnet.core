namespace Sample.Application.Services
{
    public interface IAccountService : IStrongService<Account>
    {
        string HashPassword(string password, string hashKey);

        Task<bool> AnyByUsername(string username, bool includeDeleted);

        Task<TResult?> GetByUsername<TResult>(string username, bool includeDeleted)
            where TResult : class;

        Task<TResult?> GetByUsernamePassword<TResult>(string username, string password, bool includeDeleted)
            where TResult : class;
    }

    public class AccountService : StrongService<Account>, IAccountService
    {
        public AccountService(IDbDataContext db)
            : base(db)
        {
        }

        public static string HashPasswordWithKey(string password, string hashKey)
        {
            return HashHelper.MD5Hash(password + hashKey);
        }

        public string HashPassword(string password, string hashKey)
        {
            return HashPasswordWithKey(password, hashKey);
        }

        public Task<bool> AnyByUsername(string username, bool includeDeleted)
        {
            var query = Query
                .WhereIf(!includeDeleted, x => x.IsDeleted == false)
                .Where(x => x.Auths.Any(auth => auth.Username == username));
            return Any(query);
        }

        public Task<TResult?> GetByUsername<TResult>(string username, bool includeDeleted)
            where TResult : class
        {
            var query = Query
                .WhereIf(!includeDeleted, x => x.IsDeleted == false)
                .Where(x => x.Auths.Any(auth => auth.Username == username));
            return GetFirstOrDefault<TResult>(query);
        }

        public async Task<TResult?> GetByUsernamePassword<TResult>(string username, string password, bool includeDeleted)
            where TResult : class
        {
            var account = await GetByUsername<Account>(username, includeDeleted);
            if (account == null)
                return null;

            var auth = account.Auths
                .Where(x => !x.IsDeleted)
                .FirstOrDefault();
            if (auth == null || auth.HashPasswordKey == null)
                return null;
            if (auth.HashedPassword != HashPassword(password, auth.HashPasswordKey))
                return null;

            return Mapper.MapData<TResult>(account);
        }
    }
}
