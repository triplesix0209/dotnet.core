namespace Sample.WebApi.Controllers.Admins
{
    [SwaggerTag("chi nhánh")]
    [ReadEndpoint(typeof(SiteController), typeof(Site), typeof(SiteDataAdminDto), typeof(SiteFilterAdminDto))]
    public class SiteController : AdminController
    {
    }
}
