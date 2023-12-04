using Microsoft.AspNetCore.Mvc;

namespace Sample.WebApi.Controllers.Admins.Auto
{
    [Route("Admin/_[controller]")]
    //[Authorize(AuthenticationSchemes = "account-token")]
    //[AccessLevelRequirement(MinimumAccountLevel = (int)AccountLevels.Admin)]
    public class MetadataController : BaseAdminMetadataController
    {
    }
}
