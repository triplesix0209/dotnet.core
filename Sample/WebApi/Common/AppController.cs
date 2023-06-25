namespace Sample.WebApi.Common
{
    [Route("App/[controller]")]
    [ApiExplorerSettings(GroupName = "app")]
    public abstract class AppController : BaseController
    {
    }
}