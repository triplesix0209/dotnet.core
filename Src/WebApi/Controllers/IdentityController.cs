namespace Sample.WebApi.Controllers.Commons
{
    [Authorize]
    [SwaggerTag("xác thực")]
    public class IdentityController : BaseController
    {
        public IIdentityService IdentityService { get; set; }

        public IdentityContext IdentityContext { get; set; }

        [HttpPost("Register")]
        [Transactional]
        [AllowAnonymous]
        [SwaggerOperation("Đăng ký")]
        public async Task<DataResult<IdentityRegisterResultDto>> Register([FromBody] IdentityRegisterInputDto input)
        {
            var result = await IdentityService.Register(input);
            return DataResult(result);
        }

        [HttpPost("Login")]
        [Transactional]
        [AllowAnonymous]
        [SwaggerOperation("Đăng nhập")]
        public async Task<DataResult<IdentityTokenDto>> Login([FromBody] IdentityLoginDto input)
        {
            var result = await IdentityService.LoginByUsernamePassword(input.Username, input.Password);
            return DataResult(result);
        }

        [HttpGet]
        [SwaggerOperation("Lấy thông tin tài khoản")]
        public async Task<DataResult<IdentityProfileDto>> GetProfile()
        {
            var result = await IdentityService.GetProfileByAccountId(IdentityContext.UserId!.Value);
            return DataResult(result);
        }

        [HttpPut("RefreshToken")]
        [Transactional]
        [AllowAnonymous]
        [SwaggerOperation("Gia hạn phiên đăng nhập")]
        public async Task<DataResult<IdentityTokenDto>> RefreshToken([FromBody] IdentityRefreshDto input)
        {
            var result = await IdentityService.RefreshToken(input.RefreshToken);
            return DataResult(result);
        }

        [HttpDelete]
        [Transactional]
        [SwaggerOperation("Đăng xuất")]
        public async Task<SuccessResult> LogoutAsync()
        {
            await IdentityService.ClearSession(IdentityContext.UserId!.Value);
            return SuccessResult();
        }
    }
}