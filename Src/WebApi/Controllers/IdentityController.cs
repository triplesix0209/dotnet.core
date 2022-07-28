namespace Sample.WebApi.Controllers.Commons
{
    [Authorize]
    [ValidateInput]
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
        [Transactional]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] IdentityLoginDto input)
        {
            var result = await IdentityService.LoginByUsernamePassword(input.Username, input.Password);
            return DataResult(result);
        }

        [HttpPut("RefreshToken")]
        [Transactional]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] IdentityRefreshDto input)
        {
            var result = await IdentityService.RefreshToken(input.RefreshToken);
            return DataResult(result);
        }

        [HttpDelete]
        [Transactional]
        public async Task<IActionResult> LogoutAsync()
        {
            await IdentityService.ClearSession(IdentityContext.UserId!.Value);
            return SuccessResult();
        }
    }
}