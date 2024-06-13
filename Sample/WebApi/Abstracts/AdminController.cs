namespace Sample.WebApi.Abstracts
{
    [Route("Admin/[controller]")]
    [Authorize]
    [RequireScope("admin")]
    [RequireIssuer("SAMPLE")]
    [SwaggerTagGroup("Admin", 2)]
    public abstract class AdminController : BaseController
    {
    }
}