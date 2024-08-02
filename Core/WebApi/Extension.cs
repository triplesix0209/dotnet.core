using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using Hangfire;
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
using TripleSix.Core.Identity;
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
        public static IMvcBuilder AddMvcServices(
            this IServiceCollection services,
            Assembly assembly,
            Action<MvcOptions>? configureMvc = null,
            Action<ApplicationPartManager>? configureApplicationPartManager = null)
        {
            return services
                .AddCors()
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
        /// <param name="identitySetting"><see cref="IdentityAppsetting"/>.</param>
        /// <param name="webApiAppsetting"><see cref="WebApiAppsetting"/>.</param>
        /// <param name="getSigningKeyMethod">Hàm lấy signing key.</param>
        /// <returns><see cref="IServiceCollection"/>.</returns>
        public static AuthenticationBuilder AddJwtAccessToken(
            this AuthenticationBuilder authenticationBuilder,
            IdentityAppsetting identitySetting,
            WebApiAppsetting webApiAppsetting,
            Func<IdentityAppsetting, JwtSecurityToken, string?>? getSigningKeyMethod = null)
        {
            return authenticationBuilder.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = identitySetting.ValidateIssuer,
                    ValidIssuer = identitySetting.Issuer,
                    ValidateAudience = identitySetting.ValidateAudience,
                    ValidAudiences = identitySetting.Audience,
                };

                var tokenValidator = new IdentitySecurityTokenHandler(identitySetting) { GetSigningKeyMethod = getSigningKeyMethod };
                options.SecurityTokenValidators.Clear();
                options.SecurityTokenValidators.Add(tokenValidator);

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
                            context.ErrorDescription = "Access token bị sai hoặc không phù hợp";

                        // expired tokens case
                        if (context.AuthenticateFailure != null && context.AuthenticateFailure.GetType() == typeof(SecurityTokenExpiredException))
                            context.ErrorDescription = $"Access token đã hết hạn";

                        // write response
                        if (webApiAppsetting.AllowedOrigins.Contains("*"))
                            context.Response.Headers.AccessControlAllowOrigin = "*";
                        else if (context.Request.Headers.Origin.Any() && webApiAppsetting.AllowedOrigins.Contains(context.Request.Headers.Origin.First()))
                            context.Response.Headers.AccessControlAllowOrigin = context.Request.Headers.Origin.First();
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = 401;
                        var errorResult = new ErrorResult(context.Response.StatusCode, context.Error, context.ErrorDescription).ToJson();
                        return context.Response.WriteAsync(errorResult!);
                    },
                    OnForbidden = context =>
                    {
                        if (webApiAppsetting.AllowedOrigins.Contains("*"))
                            context.Response.Headers.AccessControlAllowOrigin = "*";
                        else if (context.Request.Headers.Origin.Any() && webApiAppsetting.AllowedOrigins.Contains(context.Request.Headers.Origin.First()))
                            context.Response.Headers.AccessControlAllowOrigin = context.Request.Headers.Origin.First();
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = 403;
                        var errorResult = new ErrorResult(context.Response.StatusCode, "access_denied", "Phiên truy cập bị từ chối").ToJson();
                        return context.Response.WriteAsync(errorResult!);
                    },
                };
            });
        }

        /// <summary>
        /// Cấu hình JWT Access Token.
        /// </summary>
        /// <param name="authenticationBuilder"><see cref="AuthenticationBuilder"/>.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        /// <param name="getSigningKeyMethod">Hàm lấy signing key.</param>
        /// <returns><see cref="IServiceCollection"/>.</returns>
        public static AuthenticationBuilder AddJwtAccessToken(this AuthenticationBuilder authenticationBuilder, IConfiguration configuration, Func<IdentityAppsetting, JwtSecurityToken, string?>? getSigningKeyMethod = null)
        {
            return AddJwtAccessToken(authenticationBuilder, new IdentityAppsetting(configuration), new WebApiAppsetting(configuration), getSigningKeyMethod);
        }

        /// <summary>
        /// Cấu hình Swagger.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        /// <param name="setting"><see cref="SwaggerAppsetting"/>.</param>
        /// <param name="setupAction">Hàm tùy chỉnh Swagger.</param>
        /// <returns><see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services, SwaggerAppsetting setting, Action<SwaggerGenOptions, SwaggerAppsetting>? setupAction = null)
        {
            if (!setting.Enable) return services;

            return services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("openapi", new OpenApiInfo { Title = setting.Title, Version = setting.Version });
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

                setupAction?.Invoke(options, setting);
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
            return AddSwagger(services, new SwaggerAppsetting(configuration), setupAction);
        }

        /// <summary>
        /// Cấu hình Hangfire worker.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        /// <param name="setting"><see cref="HangfireAppsetting"/>.</param>
        /// <param name="setup">Hàm cấu hình hangfire.</param>
        /// <returns><see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddHangfireWorker(this IServiceCollection services, HangfireAppsetting setting, Action<IGlobalConfiguration, HangfireAppsetting> setup)
        {
            if (!setting.Enable) return services;

            services.AddHangfire(options =>
            {
                options.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseRecommendedSerializerSettings()
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseIgnoredAssemblyVersionTypeResolver();
                setup(options, setting);
            });

            return services.AddHangfireServer(options =>
            {
                options.Queues = setting.Queues;
                if (setting.WorkerCount.HasValue) options.WorkerCount = setting.WorkerCount.Value;
            });
        }

        /// <summary>
        /// Cấu hình Hangfire worker.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        /// <param name="setup">Hàm cấu hình hangfire.</param>
        /// <returns><see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddHangfireWorker(this IServiceCollection services, IConfiguration configuration, Action<IGlobalConfiguration, HangfireAppsetting> setup)
        {
            return AddHangfireWorker(services, new HangfireAppsetting(configuration), setup);
        }

        /// <summary>
        /// Sử dụng Redoc làm API Document.
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/>.</param>
        /// <param name="setting"><see cref="SwaggerAppsetting"/>.</param>
        /// <returns><see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder UseReDocUI(this IApplicationBuilder app, SwaggerAppsetting setting)
        {
            if (!setting.Enable) return app;

            app.UseSwagger();
            app.UseReDoc(options =>
            {
                options.RoutePrefix = setting.Route;
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
        /// Sử dụng Redoc làm API Document.
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/>.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        /// <returns><see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder UseReDocUI(this IApplicationBuilder app, IConfiguration configuration)
        {
            return UseReDocUI(app, new SwaggerAppsetting(configuration));
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
                context.HttpContext.Response.Headers.AccessControlAllowOrigin = "*";
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
        public static IApplicationBuilder Use404JsonError(this IApplicationBuilder app, string errorCode = "endpoint_not_found", string errorMessage = "Không tìm thấy endpoint chỉ định")
        {
            return UseStatusCodeJsonError(app, 404, errorCode, errorMessage);
        }

        /// <summary>
        /// Sử dụng MVC Controller.
        /// </summary>
        /// <param name="app"><see cref="WebApplication"/>.</param>
        /// <param name="setting"><see cref="WebApiAppsetting"/>.</param>
        /// <returns><see cref="WebApplication"/>.</returns>
        public static WebApplication UseMvcService(this WebApplication app, WebApiAppsetting setting)
        {
            app.UseCors(builder =>
            {
                if (setting.AllowedOrigins.Contains("*")) builder.AllowAnyOrigin();
                else builder.WithOrigins(setting.AllowedOrigins);
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });

            app.Use(next => context =>
            {
                context.Request.EnableBuffering();
                return next(context);
            });

            app.UseRouting();
            app.MapControllers();
            app.Use404JsonError();

            app.UseMiddleware<ExceptionMiddleware>();

            return app;
        }

        /// <summary>
        /// Sử dụng MVC Controller.
        /// </summary>
        /// <param name="app"><see cref="WebApplication"/>.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        /// <returns><see cref="WebApplication"/>.</returns>
        public static WebApplication UseMvcService(this WebApplication app, IConfiguration configuration)
        {
            return app.UseMvcService(new WebApiAppsetting(configuration));
        }
    }
}
