namespace Sample.WebApi.Common
{
    [Route("Common/[controller]")]
    [ApiExplorerSettings(GroupName = "common")]
    public abstract class CommonController : BaseController
    {
    }
}