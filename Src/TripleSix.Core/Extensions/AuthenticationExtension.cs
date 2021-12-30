using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using TripleSix.Core.Exceptions;

namespace TripleSix.Core.Extensions
{
    public static class AuthenticationExtension
    {
        public static AuthenticationBuilder AddHeaderJwtToken(
            this AuthenticationBuilder builder,
            Action<JwtBearerOptions> @delegate,
            string tokenField = "token")
        {
            return builder.AddJwtBearer(options => { AddHeaderJwtTokenHandler(options, @delegate, tokenField); });
        }

        public static AuthenticationBuilder AddHeaderJwtToken(
            this AuthenticationBuilder builder,
            string authenticationScheme,
            Action<JwtBearerOptions> @delegate,
            string tokenField = "token")
        {
            return builder.AddJwtBearer(
                authenticationScheme,
                options => { AddHeaderJwtTokenHandler(options, @delegate, tokenField); });
        }

        private static void AddHeaderJwtTokenHandler(
            JwtBearerOptions options,
            Action<JwtBearerOptions> @delegate,
            string tokenField)
        {
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    context.Token = context.Request.Headers[tokenField];
                    return Task.CompletedTask;
                },

                OnAuthenticationFailed = context
                    => throw new BaseException(
                        401,
                        "unauthorized",
                        "phiên đăng nhập không chính xác hoặc đã hết hạn"),

                OnForbidden = context
                    => throw new BaseException(
                        403,
                        "forbidden",
                        "truy cập bị từ chối"),
            };

            options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
            options.TokenValidationParameters.ValidateAudience = false;
            @delegate(options);
        }
    }
}