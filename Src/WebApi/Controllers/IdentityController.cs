namespace Sample.WebApi.Controllers.Commons
{
    public class IdentityController : BaseController
    {
        public IIdentityService IdentityService { get; set; }

        public IdentityContext IdentityContext { get; set; }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var result = await IdentityService.GetProfileByAccountId(IdentityContext.UserId!.Value);
            return DataResult(result);
        }

        [HttpPost]
        [ValidateInput]
        [Transactional]
        public async Task<IActionResult> Login([FromBody] IdentityLoginDto input)
        {
            var result = await IdentityService.LoginByUsernamePassword(input.Username, input.Password);
            return DataResult(result);
        }
    }
}