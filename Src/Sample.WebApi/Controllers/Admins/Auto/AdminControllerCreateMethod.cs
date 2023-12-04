﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sample.Common;
using Sample.Common.Enum;
using TripleSix.CoreOld.AutoAdmin;
using TripleSix.CoreOld.Dto;
using TripleSix.CoreOld.Entities;
using TripleSix.CoreOld.WebApi.Filters;

namespace Sample.WebApi.Controllers.Admins.Auto
{
    [Route("Admin/[controller]")]
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize(AuthenticationSchemes = "account-token")]
    [AccessLevelRequirement(MinimumAccountLevel = (int)AccountLevels.Admin)]
    public class AdminControllerCreateMethod<TAdmin, TEntity, TCreateDto>
        : BaseAdminControllerCreateMethod<TAdmin, TEntity, TCreateDto>
        where TAdmin : class, IAdminDto
        where TEntity : class, IModelEntity
        where TCreateDto : class, IDataDto
    {
        protected override IIdentity GenerateIdentity()
        {
            return new Identity(HttpContext);
        }
    }
}
