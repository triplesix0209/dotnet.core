using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sample.Common;
using Sample.Common.Enum;

namespace Sample.WebApi.Controllers.Admins.Auto
{
    [Route("Admin/[controller]")]
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize(AuthenticationSchemes = "account-token")]
    [AccessLevelRequirement(MinimumAccountLevel = (int)AccountLevels.Admin)]
    public class AdminControllerUpdateMethod<TAdmin, TEntity, TUpdateDto>
        : BaseAdminControllerUpdateMethod<TAdmin, TEntity, TUpdateDto>
        where TAdmin : class, IAdminDto
        where TEntity : class, IModelEntity
        where TUpdateDto : class, IDataDto
    {
        protected override IIdentity GenerateIdentity()
        {
            return new Identity(HttpContext);
        }
    }
}
