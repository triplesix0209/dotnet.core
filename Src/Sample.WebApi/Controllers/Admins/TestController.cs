using Sample.Common.Dto;
using Sample.WebApi.Abstracts;
using Swashbuckle.AspNetCore.Annotations;
using TripleSix.CoreOld.AutoAdmin;

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
