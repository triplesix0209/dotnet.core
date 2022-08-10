namespace Sample.WebApi.Common
{
    [Route("Admin/[controller]")]
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize]
    public abstract class AdminController : BaseController
    {
    }
}