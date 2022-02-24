using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sample.Common;
using TripleSix.Core.AutoAdmin;
using TripleSix.Core.Dto;
using TripleSix.Core.Entities;

namespace Sample.WebApi.Controllers.Admins.Auto
{
    [Route("Admin/[controller]")]
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize(AuthenticationSchemes = "account-token")]
    public class AdminControllerDeleteMethod<TEntity>
        : BaseAdminControllerDeleteMethod<TEntity>
        where TEntity : class, IModelEntity
    {
        protected override IIdentity GenerateIdentity()
        {
            return new Identity(HttpContext);
        }
    }
}
