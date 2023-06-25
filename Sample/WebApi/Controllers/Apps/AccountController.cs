namespace Sample.WebApi.Controllers.Apps
{
    [SwaggerTag("tài khoản")]
    public class AccountController : AppController
    {
        public IAccountService AccountService { get; set; }

        [HttpGet]
        [SwaggerOperation("Lấy danh sách tài khoản")]
        public async Task<PagingResult<AccountDataDto>> GetAll(PagingInputDto input)
        {
            var result = await AccountService.GetPageActivate(input.Page, input.Size);
            return PagingResult(result);
        }
    }
}
