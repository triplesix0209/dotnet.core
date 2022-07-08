using Sample.Domain.Entities;
using TripleSix.Core.WebApi.Controllers;

namespace Sample.WebApi.Controllers
{
    public class AccountController : BaseController
    {
        public IAccountService AccountService { get; set; }

        [HttpGet]
        public async Task<IActionResult> Test()
        {
            var input = new Account
            {
                Name = "Quang Lực",
            };

            var result = await AccountService.CreateAsync(input);

            return new OkResult();
        }
    }
}