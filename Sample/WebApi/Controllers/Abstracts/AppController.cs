namespace Sample.WebApi.Common
{
    [Route("App/[controller]")]
    [Authorize]
    [SwaggerTagGroup("App", 1)]
    public abstract class AppController : BaseController
    {
    }
}