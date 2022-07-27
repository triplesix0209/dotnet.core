namespace Sample.Domain.Services
{
    public interface IAccountService : IStrongService<Account>
    {
        Task<TResult?> GetByUsername<TResult>(string username)
            where TResult : class;

        Task<TResult?> GetByUsernamePassword<TResult>(string username, string password)
            where TResult : class;
    }
}
