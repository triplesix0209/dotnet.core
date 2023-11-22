﻿using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TripleSix.Core.Appsettings;
using TripleSix.Core.Helpers;
using TripleSix.Core.Jsons;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Extension.
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// Cấu hình MVC Controller.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        /// <param name="assembly">Assembly đang thực thi.</param>
        /// <param name="configureMvc">Hàm tùy chỉnh mvc.</param>
        /// <param name="configureApplicationPartManager">Hàm tùy chỉnh application part.</param>
        /// <returns><see cref="IMvcBuilder"/>.</returns>
        public static IMvcBuilder ConfigureMvcService(
            this IServiceCollection services,
            Assembly assembly,
            Action<MvcOptions>? configureMvc = null,
            Action<ApplicationPartManager>? configureApplicationPartManager = null)
        {
            return services
                .AddMvc(options =>
                {
                    options.AllowEmptyInputInBodyModelBinding = true;
                    options.ModelBinderProviders.Insert(0, new TimestampModelBinderProvider());
                    options.Filters.Add(typeof(DtoModelBinding), 0);
                    options.Conventions.Add(new ControllerEndpointRouteConvention());
                    configureMvc?.Invoke(options);
                })
                .AddControllersAsServices()
                .ConfigureApplicationPartManager(options =>
                {
                    options.FeatureProviders.Add(new ControllerEndpointFeatureProvider(assembly));
                    configureApplicationPartManager?.Invoke(options);
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new BaseContractResolver();
                    foreach (var converter in JsonHelper.Converters)
                        options.SerializerSettings.Converters.Add(converter);
                });
        }

        /// <summary>
        /// Cấu hình JWT Access Token.
        /// </summary>
        /// <param name="authenticationBuilder"><see cref="AuthenticationBuilder"/>.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        /// <returns><see cref="IServiceCollection"/>.</returns>
        public static AuthenticationBuilder AddJwtAccessToken(this AuthenticationBuilder authenticationBuilder, IConfiguration configuration)
        {
            var appsetting = new IdentityAppsetting(configuration);
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appsetting.SigningKey));
            return authenticationBuilder.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateIssuer = appsetting.ValidateIssuer,
                    ValidIssuers = appsetting.Issuer,
                    ValidateAudience = appsetting.ValidateAudience,
                    ValidAudiences = appsetting.Audience,
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var authorizationValue = context.HttpContext.Request.Headers.Authorization.FirstOrDefault();
                        if (authorizationValue == null) return Task.CompletedTask;

                        context.Token = authorizationValue.Split(' ')[^1];
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();

                        // ensure error info
                        if (context.Error.IsNullOrEmpty())
                            context.Error = "invalid_token";
                        if (context.ErrorDescription.IsNullOrEmpty())
                            context.ErrorDescription = "This request requires a valid access token to be provided";

                        // expired tokens case
                        if (context.AuthenticateFailure != null && context.AuthenticateFailure.GetType() == typeof(SecurityTokenExpiredException))
                            context.ErrorDescription = $"The access token has expired";

                        // write response
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        var errorResult = new ErrorResult(context.Response.StatusCode, context.Error, context.ErrorDescription).ToJson();
                        return context.Response.WriteAsync(errorResult!);
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        var errorResult = new ErrorResult(context.Response.StatusCode, "access_denied", "Your access is denied").ToJson();
                        return context.Response.WriteAsync(errorResult!);
                    },
                };
            });
        }

        /// <summary>
        /// Cấu hình Swagger.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        /// <param name="setupAction">Hàm tùy chỉnh Swagger.</param>
        /// <returns><see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration, Action<SwaggerGenOptions, SwaggerAppsetting>? setupAction = null)
        {
            var appsetting = new SwaggerAppsetting(configuration);
            if (!appsetting.Enable) return services;

            return services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("openapi", new OpenApiInfo { Title = appsetting.Title, Version = appsetting.Version });
                options.AddSecurityDefinition("AccessToken", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Description = "Nhập `Access Token` vào header để truy cập",
                });

                options.SwaggerGeneratorOptions.DescribeAllParametersInCamelCase = true;
                options.CustomSchemaIds(x => x.FullName);
                options.EnableAnnotations();

                options.MapType<DateTime>(() => new OpenApiSchema { Type = "integer", Format = "int64" });
                options.MapType<DateTime?>(() => new OpenApiSchema { Type = "integer", Format = "int64", Nullable = true });

                options.DocumentFilter<BaseDocumentFilter>();
                options.OperationFilter<DescribeOperationFilter>();

                setupAction?.Invoke(options, appsetting);
            });
        }

        /// <summary>
        /// Sử dụng Redoc làm API Document.
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/>.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        /// <returns><see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder UseReDocUI(this IApplicationBuilder app, IConfiguration configuration)
        {
            var appsetting = new SwaggerAppsetting(configuration);
            if (!appsetting.Enable) return app;

            app.UseSwagger();
            app.UseReDoc(options =>
            {
                options.RoutePrefix = appsetting.Route;
                options.IndexStream = () =>
                {
                    var assembly = AppDomain.CurrentDomain.GetAssemblies()
                        .First(x => x.GetName().Name == $"{nameof(TripleSix)}.{nameof(Core)}");
                    var streamName = assembly.GetManifestResourceNames()
                        .First(x => x.EndsWith("ReDoc.html"));
                    return assembly.GetManifestResourceStream(streamName);
                };
            });

            return app;
        }

        /// <summary>
        /// Cấu hình hiển thị lỗi.
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/>.</param>
        /// <param name="statusCode">Status Code sẽ xử lý.</param>
        /// <param name="errorCode">Mã lỗi.</param>
        /// <param name="errorMessage">Thông báo lỗi.</param>
        /// <returns><see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder UseStatusCodeJsonError(this IApplicationBuilder app, int statusCode, string errorCode, string errorMessage)
        {
            return app.UseStatusCodePages(async context =>
            {
                if (context.HttpContext.Response.StatusCode != statusCode)
                {
                    await context.Next(context.HttpContext);
                    return;
                }

                var result = new ErrorResult(statusCode, errorCode, errorMessage).ToJson();
                context.HttpContext.Response.ContentType = "application/json";
                await context.HttpContext.Response.WriteAsync(result!);
            });
        }

        /// <summary>
        /// Hiển thị lỗi đối với 404 Not Found.
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/>.</param>
        /// <param name="errorCode">Mã lỗi.</param>
        /// <param name="errorMessage">Thông báo lỗi.</param>
        /// <returns><see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder Use404JsonError(this IApplicationBuilder app, string errorCode = "endpoint_not_found", string errorMessage = "Request endpopint not found")
        {
            return UseStatusCodeJsonError(app, 404, errorCode, errorMessage);
        }
    }
}