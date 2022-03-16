using Sample.Common.Dto;
using Sample.WebApi.Abstracts;
using Swashbuckle.AspNetCore.Annotations;
using TripleSix.Core.AutoAdmin;

namespace Sample.WebApi.Controllers.Admins
{
    [SwaggerTag("soldier")]
    [AdminController(
        AdminType = typeof(SoldierAdminDto),
        PermissionGroup = "account")]
    public class SoldierController : AdminController
    {
    }
}
