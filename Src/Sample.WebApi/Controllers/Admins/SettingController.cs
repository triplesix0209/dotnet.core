using Sample.Common.Dto;
using Sample.Data.Entities;
using Sample.WebApi.Abstracts;
using Swashbuckle.AspNetCore.Annotations;
using TripleSix.Core.AutoAdmin;

namespace Sample.WebApi.Controllers.Admins
{
    [SwaggerTag("thiết lập")]
    [AdminController(
        AdminType = typeof(SettingAdminDto),
        EntityType = typeof(SettingEntity),
        GroupName = "hệ thống")]
    public class SettingController : AdminController
    {
    }
}
