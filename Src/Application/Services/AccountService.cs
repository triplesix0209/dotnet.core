namespace Sample.Application.Services
{
    public class AccountService : IAccountService
    {
        public async Task<string> Test()
        {
            return "Hello World";
        }
    }
}
