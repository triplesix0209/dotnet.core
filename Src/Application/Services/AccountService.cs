namespace Sample.Application.Services
{
    public interface IAccountService : IStrongService<Account>
    {
        Task<bool> AnyByUsername(string username, bool includeDeleted);

        Task<TResult?> GetByUsername<TResult>(string username, bool includeDeleted)
            where TResult : class;

        Task<TResult?> GetByUsernamePassword<TResult>(string username, string password, bool includeDeleted)
            where TResult : class;
    }

    public class AccountService : StrongService<Account>, IAccountService
    {
        public Task<bool> AnyByUsername(string username, bool includeDeleted)
        {
            var query = Db.Account
                .WhereIf(!includeDeleted, x => x.IsDeleted == false)
                .Where(x => x.Auths.Any(auth => auth.Username == username));
            return Any(query);
        }

        public Task<TResult?> GetByUsername<TResult>(string username, bool includeDeleted)
            where TResult : class
        {
            var query = Db.Account
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
            if (auth.HashedPassword != PasswordHelper.HashPassword(password, auth.HashPasswordKey))
                return null;

            return Mapper.MapData<TResult>(account);
        }
    }
}
