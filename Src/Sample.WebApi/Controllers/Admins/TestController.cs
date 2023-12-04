using Sample.Common.Dto;
using Sample.WebApi.Abstracts;

namespace Sample.WebApi.Controllers.Admins
{
    [SwaggerTag("test")]
    [AdminController(
        AdminType = typeof(TestAdminDto),
        GroupName = "hệ thống")]
    public class TestController : AdminController
    {
    }
}
