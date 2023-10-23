using Microsoft.AspNetCore.Authorization;

namespace Sample.WebApi.Common
{
    [Route("Admin/[controller]")]
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize]
    [RequireScope("admin")]
    public abstract class AdminController : BaseController
    {
    }
}