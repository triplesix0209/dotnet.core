using Sample.Common.Dto;
using Sample.WebApi.Abstracts;

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
