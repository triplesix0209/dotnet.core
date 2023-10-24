namespace Sample.WebApi.Common
{
    [Route("App/[controller]")]
    [ApiExplorerSettings(GroupName = "app")]
    [Authorize]
    public abstract class AppController : BaseController
    {
    }
}