using Sample.Common.Dto;
using Sample.WebApi.Abstracts;
using Swashbuckle.AspNetCore.Annotations;
using TripleSix.Core.AutoAdmin;

namespace Sample.WebApi.Controllers.Admins
{
    [SwaggerTag("test")]
    [AdminController(
        AdminType = typeof(TestAdminDto),
        GroupName = "hệ thống",
        EnableCreate = false,
        EnableDelete = false)]
    public class TestController : AdminController
    {
    }
}
