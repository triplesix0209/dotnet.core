namespace Sample.WebApi.Controllers.Admins
{
    [SwaggerTag("thiết lập")]
    [AdminController(
        ModelType = typeof(SettingAdminDto),
        EnableCreate = false,
        EnableDelete = false)]
    public class SettingController : AdminController
    {
    }
}
