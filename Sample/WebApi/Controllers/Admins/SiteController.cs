using TripleSix.Core.Exceptions;

namespace Sample.WebApi.Controllers.Admins
{
    [SwaggerTag("chi nhánh")]
    [ReadEndpoint<SiteController, Site, SiteDataAdminDto, SiteFilterAdminDto>]
    [CreateEndpoint<SiteController, Site, SiteCreateAdminDto>]
    public class SiteController : AdminController
    {
        [HttpGet("Test")]
        public async Task<SuccessResult> Test()
        {
            throw new NotFoundException<Site>();
        }
    }
}
