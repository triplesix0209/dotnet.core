namespace Sample.WebApi.Controllers.Admins
{
    [SwaggerTag("chi nhánh")]
    [ReadEndpoint<SiteController, Site, SiteDataAdminDto, SiteFilterAdminDto>]
    [CreateEndpoint<SiteController, Site, SiteCreateAdminDto>]
    public class SiteController : AdminController
    {
    }
}
