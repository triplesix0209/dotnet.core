using Sample.Common.Dto;
using Sample.WebApi.Abstracts;
using Swashbuckle.AspNetCore.Annotations;
using TripleSix.CoreOld.AutoAdmin;

namespace Sample.WebApi.Controllers.Admins
{
    [SwaggerTag("thiết lập")]
    [AdminController(
        AdminType = typeof(SettingAdminDto),
        GroupName = "hệ thống",
        EnableCreate = false,
        EnableDelete = false)]
    public class SettingController : AdminController
    {
    }
}
