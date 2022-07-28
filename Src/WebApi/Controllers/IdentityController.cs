namespace Sample.WebApi.Controllers.Commons
{
    [Authorize]
    [SwaggerTag("xác thực")]
    public class IdentityController : BaseController
    {
        public IIdentityService IdentityService { get; set; }

        public IdentityContext IdentityContext { get; set; }

        [HttpGet]
        [SwaggerApi("Lấy thông tin tài khoản", typeof(DataResult<IdentityTokenDto>))]
        public async Task<IActionResult> GetProfile()
        {
            var result = await IdentityService.GetProfileByAccountId(IdentityContext.UserId!.Value);
            return DataResult(result);
        }

        [HttpPost]
        [Transactional]
        [AllowAnonymous]
        [SwaggerApi("Đăng nhập", typeof(DataResult<IdentityTokenDto>))]
        public async Task<IActionResult> Login([FromBody] IdentityLoginDto input)
        {
            var result = await IdentityService.LoginByUsernamePassword(input.Username, input.Password);
            return DataResult(result);
        }

        [HttpPut("RefreshToken")]
        [Transactional]
        [AllowAnonymous]
        [SwaggerApi("Gia hạn phiên đăng nhập", typeof(DataResult<IdentityTokenDto>))]
        public async Task<IActionResult> RefreshToken([FromBody] IdentityRefreshDto input)
        {
            var result = await IdentityService.RefreshToken(input.RefreshToken);
            return DataResult(result);
        }

        [HttpDelete]
        [Transactional]
        [SwaggerApi("Đăng xuất")]
        public async Task<IActionResult> LogoutAsync()
        {
            await IdentityService.ClearSession(IdentityContext.UserId!.Value);
            return SuccessResult();
        }
    }
}