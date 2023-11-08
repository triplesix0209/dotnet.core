namespace Sample.WebApi.Controllers.Admins
{
    [SwaggerTag("tài khoản")]
    public class AccountController : AdminController
    {
        public IAccountService AccountService { get; set; }

        [HttpGet("{id}")]
        [SwaggerOperation("Lấy chi tiết tài khoản")]
        public async Task<DataResult<AccountDataAdminDto>> GetDetail(RouteId route)
        {
            var result = await AccountService.GetFirstById<AccountDataAdminDto>(route.Id);
            return DataResult(result);
        }

        [HttpPost]
        [SwaggerOperation("Tạo tài khoản")]
        [Transactional]
        public async Task<DataResult<Guid>> Create([FromBody] AccountCreateAdminDto input)
        {
            var result = await AccountService.CreateWithMapper(input);
            return DataResult(result.Id);
        }

        [HttpPut("{id}")]
        [SwaggerOperation("Sửa tài khoản")]
        [Transactional]
        public async Task<SuccessResult> Update(RouteId route, [FromBody] AccountUpdateAdminDto input)
        {
            var result = await AccountService.UpdateWithMapper(route.Id, input);
            return SuccessResult();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation("Tắt tài khoản")]
        [Transactional]
        public async Task<SuccessResult> Delete(RouteId route)
        {
            await AccountService.SoftDelete(route.Id);
            return SuccessResult();
        }

        [HttpPut("{id}/Restore")]
        [SwaggerOperation("Khôi phục tài khoản")]
        [Transactional]
        public async Task<SuccessResult> Restore(RouteId route)
        {
            await AccountService.Restore(route.Id);
            return SuccessResult();
        }
    }
}
