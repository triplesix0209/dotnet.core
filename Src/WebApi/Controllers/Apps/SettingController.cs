namespace Sample.WebApi.Controllers.Commons
{
    [SwaggerTag("thiết lập")]
    public class SettingController : AppController
    {
        public ISettingService SettingService { get; set; }

        [HttpGet]
        [SwaggerApi("Lấy danh sách thiết lập", typeof(DataResult<SettingDataDto>))]
        public async Task<IActionResult> GetAll(SettingFilterDto input)
        {
            var result = await SettingService.GetListByModel<SettingDataDto>(input);
            return DataResult(result);
        }
    }
}
