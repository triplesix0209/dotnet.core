using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripleSix.Core.AutoAdmin;

namespace Sample.WebApi.Controllers.Admins.Auto
{
    [Route("Admin/_[controller]")]
    [Authorize(AuthenticationSchemes = "account-token")]
    public class MetadataController : BaseAdminMetadataController
    {
    }
}
