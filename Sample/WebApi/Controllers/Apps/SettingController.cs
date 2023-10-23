using Microsoft.AspNetCore.Authorization;

namespace Sample.WebApi.Controllers.Apps
{
    [SwaggerTag("thiết lập")]
    [Authorize]
    public class SettingController : AppController
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
