namespace Sample.WebApi.Common
{
    [Route("App/[controller]")]
    [ApiExplorerSettings(GroupName = "common")]
    [Authorize]
    public abstract class AppController : BaseController
    {
    }
}