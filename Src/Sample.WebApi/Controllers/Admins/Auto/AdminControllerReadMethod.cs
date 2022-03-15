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
