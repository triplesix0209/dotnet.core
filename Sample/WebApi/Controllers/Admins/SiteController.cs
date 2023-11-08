namespace Sample.WebApi.Controllers.Admins
{
    [SwaggerTag("chi nhánh")]
    [ReadEndpoint<SiteController, Site, SiteDataAdminDto, SiteFilterAdminDto>]
    public class SiteController : AdminController
    {
    }
}
