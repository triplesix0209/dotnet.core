namespace Sample.WebApi.Common
{
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = "app")]
    [Authorize]
    [RequireScope("admin")]
    public abstract class CommonController : BaseController
    {
    }
}