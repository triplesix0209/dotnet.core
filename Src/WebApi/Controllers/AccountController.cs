using Sample.Domain.Entities;
using Sample.Domain.Persistences;
using TripleSix.Core.WebApi.Controllers;

namespace Sample.WebApi.Controllers
{
    public class AccountController : BaseController
    {
        public IApplicationDbContext Db { get; set; }

        public IAccountService AccountService { get; set; }

        [HttpGet]
        public async Task<Account> Test()
        {
            //var input = new Account
            //{
            //    Name = "Quang Lực",
            //};

            //await AccountService.CreateAsync(input);
            //var data = await AccountService.GetFirstAsync();

            //return data;
            return null;
        }
    }
}