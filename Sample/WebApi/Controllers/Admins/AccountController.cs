namespace Sample.WebApi.Controllers.Admins
{
    [SwaggerTag("tài khoản")]
    [ReadEndpoint(typeof(AccountController), typeof(Account), typeof(AccountDataAdminDto), typeof(AccountFilterAdminDto))]
    [CreateEndpoint(typeof(AccountController), typeof(Account), typeof(AccountCreateAdminDto))]
    public class AccountController : AdminController
    {
        public IAccountService AccountService { get; set; }

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
