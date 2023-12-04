using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sample.Common;
using Sample.Common.Dto;
using Sample.Middle.Services;
using Sample.WebApi.Abstracts;
using Swashbuckle.AspNetCore.Annotations;
using TripleSix.CoreOld.Attributes;
using TripleSix.CoreOld.WebApi.Results;

namespace Sample.WebApi.Controllers.Commons
{
    [SwaggerTag("thiết lập")]
    public class SettingController : CommonController
    {
        public ISettingService SettingService { get; set; }

        [HttpGet]
        [SwaggerApi("lấy tất cả các thiết lập", typeof(DataResult<SettingDataDto[]>))]
        public async Task<IActionResult> GetAll(SettingFilterDto input)
        {
            var identity = GenerateIdentity<Identity>();
            var data = await SettingService.GetListByFilter<SettingDataDto>(identity, input);
            return DataResult(data);
        }
    }
}
