using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sample.Common;
using Sample.Common.Enum;
using TripleSix.Core.AutoAdmin;
using TripleSix.Core.Dto;
using TripleSix.Core.Entities;
using TripleSix.Core.WebApi.Filters;

namespace Sample.WebApi.Controllers.Admins.Auto
{
    [Route("Admin/[controller]")]
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize(AuthenticationSchemes = "account-token")]
    [AccessLevelRequirement(MinimumAccountLevel = (int)AccountLevels.Admin)]
    public class AdminControllerChangeLogMethod<TEntity>
        : BaseAdminControllerChangeLogMethod<TEntity>
        where TEntity : class, IModelEntity
    {
        protected override IIdentity GenerateIdentity()
        {
            return new Identity(HttpContext);
        }
    }
}
