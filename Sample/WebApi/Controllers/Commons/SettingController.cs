namespace Sample.WebApi.Controllers.Commons
{
    [SwaggerTag("thiết lập")]
    public class SettingController : CommonController
    {
        public ISettingService SettingService { get; set; }

        [HttpGet]
        [SwaggerOperation("Lấy danh sách thiết lập")]
        public async Task<DataResult<List<SettingDataDto>>> GetAll(SettingFilterDto input)
        {
            var result = await SettingService.GetListByQueryModel<SettingDataDto>(input);
            return DataResult(result);
        }
    }
}
