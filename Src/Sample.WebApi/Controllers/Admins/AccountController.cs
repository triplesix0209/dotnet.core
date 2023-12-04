using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sample.Common;
using Sample.Common.Dto;
using Sample.Middle.Services;
using Sample.WebApi.Abstracts;

namespace Sample.WebApi.Controllers.Admins
{
    [SwaggerTag("tài khoản")]
    [AdminController(
        AdminType = typeof(AccountAdminDto),
        GroupName = "hệ thống")]
    public class AccountController : AdminController
    {
        public IAccountService AccountService { get; set; }

        [HttpPost("{id}/Verify/Email")]
        [SwaggerOperation("gửi mail xác thực")]
        [SwaggerResponse(200, null, typeof(SuccessResult))]
        [Transactional]
        public async Task<IActionResult> GenerateVerifyByEmail(RouteId route)
        {
            var identity = GenerateIdentity<Identity>();
            await AccountService.GenerateEmailVerify(identity, route.Id, true);
            return new SuccessResult();
        }
    }
}
