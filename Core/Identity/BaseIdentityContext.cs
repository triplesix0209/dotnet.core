﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TripleSix.Core.Appsettings;

namespace TripleSix.Core.Identity
{
    public abstract class BaseIdentityContext : IIdentityContext
    {
        /// <summary>
        /// Khởi tạo <see cref="BaseIdentityContext"/>.
        /// </summary>
        /// <param name="httpContext"><see cref="HttpContext"/>.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        protected BaseIdentityContext(HttpContext? httpContext, IConfiguration configuration)
        {
            if (httpContext == null) return;

            var accessToken = GetAccessToken(httpContext);
            if (accessToken == null) return;

            try
            {
                var claims = ValidateAccessToken(accessToken, configuration);
                ParseData(claims);
            }
            catch
            {
                Id = null;
            }
        }

        public Guid? Id { get; set; }

        public IEnumerable<string> Scope { get; set; }

        public virtual string? GetAccessToken(HttpContext httpContext)
        {
            var accessToken = httpContext.Request.Headers.Authorization.FirstOrDefault();
            if (accessToken == null) return null;

            if (accessToken.Split(" ").Length > 1) accessToken = accessToken.Split(" ")[1];
            return accessToken;
        }

        public virtual IEnumerable<Claim> ValidateAccessToken(string accessToken, IConfiguration configuration)
        {
            var appsetting = new IdentityAppsetting(configuration);
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appsetting.SigningKey));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = appsetting.ValidateIssuer,
                ValidIssuer = appsetting.Issuer,
                ValidateAudience = appsetting.ValidateAudience,
                ValidAudience = appsetting.Audience,
            };

            return new JwtSecurityTokenHandler()
                .ValidateToken(accessToken, tokenValidationParameters, out _)
                .Claims;
        }

        /// <summary>
        /// Đọc và nhận thông tin của Token.
        /// </summary>
        /// <param name="claims">Danh sách các <see cref="Claim"/>.</param>
        protected abstract void ParseData(IEnumerable<Claim> claims);
    }
}
