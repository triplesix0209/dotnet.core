﻿using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TripleSix.Core.Appsettings;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Xác thực với access token.
    /// </summary>
    public class AccessTokenSchemeHandler : AuthenticationHandler<AccessTokenSchemeOption>
    {
        private readonly IdentityAppsetting _appsetting;

        public AccessTokenSchemeHandler(
            IOptionsMonitor<AccessTokenSchemeOption> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IConfiguration configuration)
            : base(options, logger, encoder, clock)
        {
            _appsetting = new IdentityAppsetting(configuration);
        }

        /// <inheritdoc/>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                var accessToken = Request.Headers.ContainsKey(Options.AccessTokenHeaderKey)
                    ? Request.Headers[Options.AccessTokenHeaderKey].ToString().Trim()
                    : null;
                if (accessToken != null && accessToken.StartsWith("bearer ", StringComparison.CurrentCultureIgnoreCase))
                    accessToken = accessToken[7..];
                if (accessToken.IsNullOrWhiteSpace())
                    throw new Exception("access token not found");

                IEnumerable<Claim>? claims;
                if (Options.ValidateAccessToken)
                {
                    var validationResult = JwtHelper.ValidateJwtToken(accessToken, _appsetting.SecretKey, _appsetting.Issuer);
                    if (!validationResult.IsValid) throw validationResult.Exception;
                    claims = validationResult.ClaimsIdentity.Claims;
                }
                else
                {
                    claims = JwtHelper.ReadJwtToken(accessToken)?.Claims;
                }

                var ticket = new AuthenticationTicket(
                    new ClaimsPrincipal(new ClaimsIdentity(claims, nameof(AccessTokenSchemeHandler))),
                    Scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            catch (Exception e)
            {
                return Task.FromResult(AuthenticateResult.Fail(e));
            }
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Context.Response.ContentType = "application/json";
            Context.Response.StatusCode = 401;
            await Context.Response.WriteAsync(
                new ErrorResult(401, "unauthorized", "lỗi xác thực, xin vui lòng đăng nhập để tiếp tục").ToJson() !,
                Encoding.UTF8);
        }

        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Context.Response.ContentType = "application/json";
            Context.Response.StatusCode = 403;
            await Context.Response.WriteAsync(
                new ErrorResult(403, "forbidden", "bạn không được cấp phép để tiếp tục").ToJson() !,
                Encoding.UTF8);
        }
    }
}
