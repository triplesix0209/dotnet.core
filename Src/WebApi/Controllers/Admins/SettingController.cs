namespace Sample.WebApi.Controllers.Admins
{
    [SwaggerTag("thiết lập")]
    [AdminController(
        ModelType = typeof(SettingAdminDto))]
    public class SettingController : AdminController
    {
    }
}
