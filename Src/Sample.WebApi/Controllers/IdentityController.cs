using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sample.Common;
using Sample.Common.Dto;
using Sample.Data.Entities;
using Sample.Middle.Services;
using Sample.WebApi.Abstracts;
using Swashbuckle.AspNetCore.Annotations;
using TripleSix.CoreOld.Attributes;
using TripleSix.CoreOld.Dto;
using TripleSix.CoreOld.WebApi.Controllers;
using TripleSix.CoreOld.WebApi.Filters;
using TripleSix.CoreOld.WebApi.Results;

namespace Sample.WebApi.Controllers.Commons
{
    [SwaggerTag("xác thực")]
    public class IdentityController : BaseController
    {
        public IIdentityService IdentityService { get; set; }

        public IAccountService AccountService { get; set; }

        [HttpPost("Register")]
        [SwaggerApi("đăng ký bằng username & password", typeof(DataResult<Guid>))]
        [Transactional]
        public async Task<IActionResult> Register([FromBody] IdentityRegisterDto input)
        {
            var identity = GenerateIdentity<Identity>();
            var account = await AccountService.CreateWithModel<AccountEntity>(identity, input, true);
            return new DataResult<Guid>(account.Id);
        }

        [HttpPost]
        [SwaggerApi("đăng nhập bằng username & password", typeof(DataResult<IdentityTokenDto>))]
        public async Task<IActionResult> LoginByUsernamePassword([FromBody] IdentityLoginUsernamePasswordDto input)
        {
            var identity = GenerateIdentity<Identity>();
            var token = await IdentityService.LoginByUsernamePassword(identity, input);
            return new DataResult<IdentityTokenDto>(token);
        }

        [HttpPut("RefreshToken")]
        [SwaggerApi("gia hạn phiên đăng nhập", typeof(DataResult<IdentityTokenDto>))]
        [Transactional]
        public async Task<IActionResult> RefreshToken([FromBody] IdentityRefreshTokenDto input)
        {
            var identity = GenerateIdentity<Identity>();
            var token = await IdentityService.GenerateTokenBySessionToken(identity, input.RefreshToken, true);
            return new DataResult<IdentityTokenDto>(token);
        }

        [HttpDelete]
        [SwaggerApi("đăng xuất")]
        [Authorize(AuthenticationSchemes = "account-token")]
        [Transactional]
        public async Task<IActionResult> LogoutAsync()
        {
            var identity = GenerateIdentity<Identity>();
            await IdentityService.ClearSession(identity, identity.UserId.Value);
            return new SuccessResult();
        }

        [HttpGet]
        [SwaggerApi("lấy thông tin cá nhân", typeof(DataResult<IdentityProfileDto>))]
        [Authorize(AuthenticationSchemes = "account-token")]
        public async Task<IActionResult> GetInfo()
        {
            var identity = GenerateIdentity<Identity>();
            if (identity.User is null || !identity.User.Identity.IsAuthenticated)
                throw new AppException(AppExceptions.SessionInvalid);

            var profile = await IdentityService.GetProfileByAccountId(identity, identity.UserId.Value);
            return new DataResult<IdentityProfileDto>(profile);
        }

        [HttpPut]
        [SwaggerApi("thay đổi thông tin cá nhân")]
        [Authorize(AuthenticationSchemes = "account-token")]
        //[PermissionRequirement(Code = "profile.update")]
        [Transactional]
        public async Task<IActionResult> Update([FromBody] IdentityUpdateProfileDto input)
        {
            var identity = GenerateIdentity<Identity>();
            await AccountService.UpdateWithMapper(identity, identity.UserId.Value, input);
            return new SuccessResult();
        }

        [HttpPut("ChangePassword")]
        [SwaggerApi("đổi mật khẩu cá nhân")]
        [Authorize(AuthenticationSchemes = "account-token")]
        //[PermissionRequirement(Code = "profile.update")]
        [Transactional]
        public async Task<IActionResult> ChangePassword([FromBody] IdentityUpdatePasswordDto input)
        {
            var identity = GenerateIdentity<Identity>();
            await AccountService.UpdatePassword(identity, identity.UserId.Value, input.OldPassword, input.NewPassword);
            return new SuccessResult();
        }

        [HttpPost("{id}/Verify/Email")]
        [SwaggerApi("gửi mail xác thực")]
        [Transactional]
        public async Task<IActionResult> GenerateEmailVerify(RouteId route)
        {
            var identity = GenerateIdentity<Identity>();
            await AccountService.GenerateEmailVerify(identity, route.Id, true);
            return new SuccessResult();
        }

        [HttpPut("Verify/{verifyId}")]
        [SwaggerApi("xác thực tài khoản", typeof(DataResult<IdentityTokenDto>))]
        [Transactional]
        public async Task<IActionResult> Verify([SwaggerParameter("mã định danh phiên xác thực")] Guid verifyId)
        {
            var identity = GenerateIdentity<Identity>();
            var verify = await AccountService.Verify(identity, verifyId);
            var token = await IdentityService.GenerateTokenByAccountId(identity, verify.AccountId);
            return new DataResult<IdentityTokenDto>(token);
        }

        [HttpPost("ResetPassword")]
        [SwaggerApi("gửi mail reset password")]
        [Transactional]
        public async Task<IActionResult> GenerateResetPasswordVerify([FromBody] IdentityResetPasswordVerifyDto input)
        {
            var identity = GenerateIdentity<Identity>();

            var account = await AccountService.GetByEmail<AccountEntity>(identity, input.Email);
            if (account is null)
                throw new AppException(AppExceptions.AccountNotFound);
            if (account.IsDeleted)
                throw new AppException(AppExceptions.AccountInactive);

            await AccountService.GenerateResetPasswordVerify(identity, account.Id, true);
            return new SuccessResult();
        }

        [HttpPut("ResetPassword")]
        [SwaggerApi("reset password")]
        [Transactional]
        public async Task<IActionResult> ResetPassword([FromBody] IdentityResetPasswordDto input)
        {
            var identity = GenerateIdentity<Identity>();

            var verify = await AccountService.Verify(identity, input.VerifyId.Value);
            if (verify is null)
                throw new AppException(AppExceptions.VerifyInvalid);

            var account = await AccountService.GetFirstById(identity, verify.AccountId, false);
            if (account.IsDeleted)
                throw new AppException(AppExceptions.AccountInactive);

            await AccountService.SetPassword(identity, account.Id, input.Password);
            return new SuccessResult();
        }

        protected override IIdentity GenerateIdentity()
        {
            return new Identity(HttpContext);
        }
    }
}
