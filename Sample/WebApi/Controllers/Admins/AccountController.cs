namespace Sample.WebApi.Controllers.Admins
{
    [SwaggerTag("tài khoản")]
    public class AccountController : AdminController
    {
        public IAccountService AccountService { get; set; }

        [HttpGet]
        [SwaggerOperation("Lấy danh sách tài khoản")]
        public async Task<DataResult<List<AccountDataAdminDto>>> GetAll(AccountFilterAdminDto input)
        {
            var result = await AccountService.GetListByQueryModel<AccountDataAdminDto>(input);
            return DataResult(result);
        }

        [HttpPost]
        [SwaggerOperation("Tạo tài khoản")]
        [Transactional]
        public async Task<DataResult<AccountDataAdminDto>> Create([FromBody] AccountCreateAdminDto input)
        {
            var result = await AccountService.CreateWithMapper<AccountDataAdminDto>(input);
            return DataResult(result);
        }

        [HttpPut("{id}")]
        [SwaggerOperation("Sửa tài khoản")]
        [Transactional]
        public async Task<SuccessResult> Update(RouteId route, [FromBody] AccountUpdateAdminDto input)
        {
            var entity = await AccountService.GetFirstById(route.Id);
            await AccountService.UpdateWithMapper(entity, input);
            return SuccessResult();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation("Tắt tài khoản")]
        [Transactional]
        public async Task<SuccessResult> Delete(RouteId route)
        {
            var entity = await AccountService.GetFirstById(route.Id);
            await AccountService.SoftDelete(entity);
            return SuccessResult();
        }

        [HttpPut("{id}/Restore")]
        [SwaggerOperation("Khôi phục tài khoản")]
        [Transactional]
        public async Task<SuccessResult> Restore(RouteId route)
        {
            var entity = await AccountService.GetFirstById(route.Id);
            await AccountService.Restore(entity);
            return SuccessResult();
        }
    }
}
