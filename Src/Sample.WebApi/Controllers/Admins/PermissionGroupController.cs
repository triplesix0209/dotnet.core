using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sample.Common;
using Sample.Common.Dto;
using Sample.Middle.Services;
using Sample.WebApi.Abstracts;

namespace Sample.WebApi.Controllers.Admins
{
    [SwaggerTag("nhóm quyền")]
    [AdminController(
        AdminType = typeof(PermissionGroupAdminDto),
        GroupName = "hệ thống",
        PermissionGroup = "permission")]
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
