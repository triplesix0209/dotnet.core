using System;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using TripleSix.Core.Exceptions;
using TripleSix.Core.Helpers;
using TripleSix.Core.JsonSerializers;
using TripleSix.Core.WebApi.Results;

namespace TripleSix.Core.WebApi.Authentication
{
    public class TokenSchemeHandler : AuthenticationHandler<TokenSchemeOption>
    {
        private readonly IConfiguration _configuration;

        public TokenSchemeHandler(
            IOptionsMonitor<TokenSchemeOption> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IConfiguration configuration)
            : base(options, logger, encoder, clock)
        {
            _configuration = configuration;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            BaseException error = null;
            try
            {
                if (!Request.Headers.ContainsKey(HeaderNames.Authorization))
                    throw new Exception("không tìm thấy token");

                var tokenResult = new JsonWebTokenHandler().ValidateToken(
                   Request.Headers.GetValue(HeaderNames.Authorization).Replace("Bearer", string.Empty).Trim(),
                   new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.GetValue<string>("Identity:SecretKey"))),

                       ValidateIssuer = true,
                       ValidIssuer = _configuration.GetValue<string>("Identity:Issuer"),

                       ValidateAudience = false,

                       ValidateLifetime = true,
                       ClockSkew = TimeSpan.Zero,
                   });

                if (!tokenResult.IsValid)
                    throw new Exception("token không chính xác hoặc đã hết hạn");

                var ticket = new AuthenticationTicket(
                    new ClaimsPrincipal(new ClaimsIdentity(tokenResult.ClaimsIdentity.Claims, nameof(TokenSchemeHandler))),
                    Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch (Exception e)
            {
                error = new BaseException(401, "unauthorized", e.Message);
            }

            Context.Response.ContentType = "application/json";
            Context.Response.StatusCode = error.HttpCode;
            await Context.Response.WriteAsync(
                JsonHelper.SerializeObject(new ErrorResult(error.HttpCode, error.Code, error.Message)),
                Encoding.UTF8);
            return AuthenticateResult.Fail(error);
        }

        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Context.Response.ContentType = "application/json";
            Context.Response.StatusCode = 403;
            await Context.Response.WriteAsync(
                JsonHelper.SerializeObject(new ErrorResult(403, "forbidden", "bạn không được cấp phép để tiếp tục")),
                Encoding.UTF8);
        }
    }
}
