using TripleSix.Core.WebApi.Controllers;

namespace Sample.WebApi.Controllers
{
    public class AccountController : BaseController
    {
        public IAccountService AccountService { get; set; }

        [HttpGet]
        public Task<string> Test()
        {
            return AccountService.Test();
        }
    }
}