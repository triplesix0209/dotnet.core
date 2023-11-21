namespace Sample.WebApi.Controllers.Admins
{
    [SwaggerTag("chi nhánh")]
    [AdminReadEndpoint<SiteController, Site, SiteDataAdminDto, SiteDataAdminDto, SiteFilterAdminDto>]
    [AdminCreateEndpoint<SiteController, Site, SiteCreateAdminDto>]
    [AdminUpdateEndpoint<SiteController, Site, SiteUpdateAdminDto>]
    [AdminSoftDeleteEndpoint<SiteController, Site>]
    public class SiteController : AdminController
    {
    }
}
