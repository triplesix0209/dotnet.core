using Microsoft.AspNetCore.Mvc;
using TripleSix.Core.AutoAdmin;

namespace Sample.WebApi.Controllers.Admins.Auto
{
    [Route("Admin/_[controller]")]
    public class MetadataController : BaseAdminMetadataController
    {
    }
}
