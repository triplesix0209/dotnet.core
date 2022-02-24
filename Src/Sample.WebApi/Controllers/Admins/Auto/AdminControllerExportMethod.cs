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
    public class AdminControllerExportMethod<TEntity, TFilterDto, TDetailDto>
        : BaseAdminControllerExportMethod<TEntity, TFilterDto, TDetailDto>
        where TEntity : class, IModelEntity
        where TFilterDto : IModelFilterDto
        where TDetailDto : class, IModelDataDto
    {
        protected override IIdentity GenerateIdentity()
        {
            return new Identity(HttpContext);
        }
    }
}
