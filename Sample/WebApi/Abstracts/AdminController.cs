namespace Sample.WebApi.Abstracts
{
    [Route("Admin/[controller]")]
    [Authorize]
    [RequireScope("admin")]
    [RequireIssuer("IDENTITY")]
    [SwaggerTagGroup("Admin", 2)]
    public abstract class AdminController : BaseController
    {
    }
}