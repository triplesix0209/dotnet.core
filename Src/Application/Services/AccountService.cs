namespace Sample.Application.Services
{
    public class AccountService : StrongService<Account>, IAccountService
    {
        public static string HashPassword(string password, string hashKey)
        {
            return HashHelper.MD5Hash(password + hashKey);
        }

        public Task<TResult?> GetByUsername<TResult>(string username)
            where TResult : class
        {
            var query = Db.Account
                .Where(x => x.Auths.Any(auth => auth.Username == username));
            return GetFirstOrDefault<TResult>(query);
        }

        public async Task<TResult?> GetByUsernamePassword<TResult>(string username, string password)
            where TResult : class
        {
            var account = await GetByUsername<Account>(username);
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
