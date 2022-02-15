using Microsoft.AspNetCore.Mvc;
using Sample.Common;
using TripleSix.Core.AutoAdmin;
using TripleSix.Core.Dto;

namespace Sample.WebApi.Abstracts
{
    [Route("Admin/[controller]")]
    [ApiExplorerSettings(GroupName = "admin")]
    public abstract class AdminController : BaseAdminController
    {
        protected override IIdentity GenerateIdentity()
        {
            return new Identity(HttpContext);
        }
    }
}
