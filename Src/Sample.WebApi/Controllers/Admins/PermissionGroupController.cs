using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sample.Common;
using Sample.Common.Dto;
using Sample.Data.Entities;
using Sample.Middle.Services;
using Sample.WebApi.Abstracts;
using Swashbuckle.AspNetCore.Annotations;
using TripleSix.Core.Attributes;
using TripleSix.Core.AutoAdmin;
using TripleSix.Core.WebApi.Results;

namespace Sample.WebApi.Controllers.Admins
{
    [SwaggerTag("nhóm quyền")]
    [AdminController(
        AdminType = typeof(PermissionGroupAdminDto),
        EntityType = typeof(PermissionGroupEntity),
        GroupName = "hệ thống")]
    public class PermissionGroupController : AdminController
    {
        public IPermissionGroupService PermissionGroupService { get; set; }

        [HttpGet("PermissionValue")]
        [SwaggerApi("lấy danh sách quyền của nhóm", typeof(DataResult<PermissionValueDto[]>))]
        public async Task<IActionResult> GetListPermissionValue(PermissionGroupAdminDto.ListPermissionValue input)
        {
            var identity = GenerateIdentity<Identity>();
            var data = await PermissionGroupService.GetListPermissionValue(identity, input.Id);
            return DataResult(data);
        }
    }
}
