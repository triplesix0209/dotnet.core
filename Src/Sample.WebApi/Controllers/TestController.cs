using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sample.Common;
using Sample.Data.Entities;
using Sample.Middle.Services;
using Sample.WebApi.Abstracts;

namespace Sample.WebApi.Controllers.Commons
{
    public class TestController : CommonController
    {
        public ISettingService SettingService { get; set; }

        [HttpGet]
        public async Task<IActionResult> Test()
        {
            var identity = GenerateIdentity<Identity>();

            var entity = await SettingService.Create(identity, new SettingEntity
            {
                Code = "test",
                Value = null,
                Description = null,
            });
            await SettingService.WriteChangeLog(identity, entity.Id);

            return SuccessResult();
        }
    }
}
