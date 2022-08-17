﻿using TripleSix.Core.Entities;

namespace Sample.WebApi.Common
{
    [Route("Admin/[controller]")]
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize]
    [AccessLevelRequirement(MinimumAccountLevel = (int)AccountLevels.Admin)]
    public class AdminControllerReadMethod<TEntity, TAdminModel, TFilterDto, TItemDto, TDetailDto>
        : BaseAdminControllerReadMethod<TEntity, TAdminModel, TFilterDto, TItemDto, TDetailDto>
        where TEntity : class, IStrongEntity
        where TAdminModel : AdminModel<TEntity>
        where TFilterDto : BaseAdminFilterDto<TEntity>
        where TItemDto : BaseAdminItemDto
        where TDetailDto : BaseAdminItemDto
    {
    }
}
