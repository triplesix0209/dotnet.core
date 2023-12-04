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
    public class AdminControllerReadMethod<TAdmin, TEntity, TFilterDto, TItemDto, TDetailDto>
        : BaseAdminControllerReadMethod<TAdmin, TEntity, TFilterDto, TItemDto, TDetailDto>
        where TAdmin : class, IAdminDto
        where TEntity : class, IModelEntity
        where TFilterDto : IModelFilterDto
        where TItemDto : class, IModelDataDto
        where TDetailDto : class, IModelDataDto
    {
        protected override IIdentity GenerateIdentity()
        {
            return new Identity(HttpContext);
        }
    }
}
