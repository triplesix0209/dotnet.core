namespace Sample.WebApi.Controllers.Commons
{
    public class IdentityController : BaseController
    {
        public IAccountService AccountService { get; set; }

        [HttpPost]
        [ValidateInput]
        public async Task<IActionResult> Login([FromBody] IdentityLoginDto input)
        {
            var account = await AccountService.GetByUsernamePassword<Account>("admin", input.Password);
            return SuccessResult();
        }
    }
}