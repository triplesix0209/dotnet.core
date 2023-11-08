namespace Sample.WebApi.Common
{
    [Route("Admin/[controller]")]
    //[Authorize]
    //[RequireScope("admin")]
    [SwaggerTagGroup("Admin", 2)]
    public abstract class AdminController : BaseController
    {
    }
}