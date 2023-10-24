namespace Sample.WebApi.Common
{
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = "app")]
    public abstract class CommonController : BaseController
    {
    }
}