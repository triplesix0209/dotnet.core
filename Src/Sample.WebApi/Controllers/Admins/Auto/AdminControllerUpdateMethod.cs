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
    public class AdminControllerUpdateMethod<TEntity, TUpdateDto>
        : BaseAdminControllerUpdateMethod<TEntity, TUpdateDto>
        where TEntity : class, IModelEntity
        where TUpdateDto : class, IDataDto
    {
        protected override IIdentity GenerateIdentity()
        {
            return new Identity(HttpContext);
        }
    }
}
