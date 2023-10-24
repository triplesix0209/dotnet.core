namespace Sample.WebApi.Controllers.Admins
{
    [SwaggerTag("chi nhánh")]
    public class SiteController : CommonController
    {
        public ISiteService SiteService { get; set; }

        [HttpGet]
        [SwaggerOperation("Lấy danh sách chi nhánh")]
        public async Task<DataResult<List<SiteAdminDataDto>>> GetAll(SiteAdminFilterDto input)
        {
            var result = await SiteService.GetListByQueryModel<SiteAdminDataDto>(input);
            return DataResult(result);
        }

        [HttpPost]
        [SwaggerOperation("Tạo chi nhánh")]
        [Transactional]
        public async Task<DataResult<Guid>> Create([FromBody] SiteAdminCreateDto input)
        {
            var result = await SiteService.CreateWithMapper(input);
            return DataResult(result.Id);
        }

        [HttpPut("{id}")]
        [SwaggerOperation("Sửa chi nhánh")]
        [Transactional]
        public async Task<SuccessResult> Update(RouteId route, [FromBody] SiteAdminUpdateDto input)
        {
            await SiteService.UpdateWithMapper(route.Id, input);
            return SuccessResult();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation("Tắt chi nhánh")]
        [Transactional]
        public async Task<SuccessResult> Delete(RouteId route)
        {
            await SiteService.SoftDelete(route.Id);
            return SuccessResult();
        }

        [HttpPut("{id}/Restore")]
        [SwaggerOperation("Khôi phục chi nhánh")]
        [Transactional]
        public async Task<SuccessResult> Restore(RouteId route)
        {
            await SiteService.Restore(route.Id);
            return SuccessResult();
        }
    }
}
