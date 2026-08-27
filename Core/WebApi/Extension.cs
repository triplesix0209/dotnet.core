using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Swashbuckle.AspNetCore.SwaggerGen;
using TripleSix.Core.Appsettings;
using TripleSix.Core.Exceptions;
using TripleSix.Core.Hangfire;
using TripleSix.Core.Helpers;
using TripleSix.Core.Identity;

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
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(["application/json"]);
            });

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
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                    var resolver = new System.Text.Json.Serialization.Metadata.DefaultJsonTypeInfoResolver();
                    resolver.Modifiers.Add(JsonHelper.BaseContractResolverModifier);
                    options.JsonSerializerOptions.TypeInfoResolver = resolver;
                    foreach (var converter in JsonHelper.Converters)
                        options.JsonSerializerOptions.Converters.Add(converter);
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
                    ValidIssuers = identitySetting.IssuerSigningKey?.Select(x => x.Issuer),
                    ValidateAudience = identitySetting.ValidateAudience,
                    ValidAudiences = identitySetting.Audience,
                };

                var tokenValidator = new IdentitySecurityTokenHandler(identitySetting) { GetSigningKeyMethod = getSigningKeyMethod };
                options.TokenHandlers.Clear();
                options.TokenHandlers.Add(tokenValidator);

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
                        var errorResult = new ErrorResult(context.Response.StatusCode, context.Error, context.ErrorDescription).ToJsonText();
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
                        var errorResult = new ErrorResult(context.Response.StatusCode, "access_denied", "Phiên truy cập bị từ chối").ToJsonText();
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
        /// <param name="setting"><see cref="DocumentSwaggerAppsetting"/>.</param>
        /// <param name="setupAction">Hàm tùy chỉnh Swagger.</param>
        /// <returns><see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services, DocumentSwaggerAppsetting setting, Action<SwaggerGenOptions, DocumentSwaggerAppsetting>? setupAction = null)
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
                options.OrderActionsBy(apiDesc =>
                {
                    var summary = apiDesc.ActionDescriptor.EndpointMetadata.OfType<Swashbuckle.AspNetCore.Annotations.SwaggerOperationAttribute>().FirstOrDefault()?.Summary
                        ?? apiDesc.ActionDescriptor.RouteValues["action"]
                        ?? apiDesc.RelativePath;
                    return $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{summary}_{apiDesc.RelativePath}";
                });

                options.MapType<DateTime>(() => new OpenApiSchema { Type = "integer", Format = "int64" });
                options.MapType<DateTime?>(() => new OpenApiSchema { Type = "integer", Format = "int64", Nullable = true });

                options.DocumentFilter<BaseDocumentFilter>();
                options.OperationFilter<DescribeOperationFilter>();
                options.SchemaFilter<DescribeSchemaFilter>();

                setupAction?.Invoke(options, setting);
            });
        }

        /// <summary>
        /// Cấu hình Swagger.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        /// <param name="setting"><see cref="DocumentAppsetting"/>.</param>
        /// <param name="setupAction">Hàm tùy chỉnh Swagger.</param>
        /// <returns><see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services, DocumentAppsetting setting, Action<SwaggerGenOptions, DocumentSwaggerAppsetting>? setupAction = null)
        {
            return AddSwagger(services, setting.Swagger, setupAction);
        }

        /// <summary>
        /// Cấu hình Swagger.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        /// <param name="setupAction">Hàm tùy chỉnh Swagger.</param>
        /// <returns><see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration, Action<SwaggerGenOptions, DocumentSwaggerAppsetting>? setupAction = null)
        {
            return AddSwagger(services, new DocumentAppsetting(configuration).Swagger, setupAction);
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
            services.AddHangfire(options =>
            {
                options.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseRecommendedSerializerSettings()
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseIgnoredAssemblyVersionTypeResolver();
                setup(options, setting);
            });

            GlobalJobFilters.Filters.Add(new SkipRetryOnExceptionAttribute(typeof(JobException)));

            return services;
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
        /// Cài đặt endpoint tài liệu hệ thống.
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/>.</param>
        /// <param name="setting"><see cref="DocumentAppsetting"/>.</param>
        /// <returns><see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder UseDocument(
            this IApplicationBuilder app,
            DocumentAppsetting setting)
        {
            if (!setting.Page.Enable && !setting.Swagger.Enable) return app;

            var baseRoute = setting.Route.Trim('/');
            if (baseRoute.IsNullOrEmpty()) baseRoute = "_document";

            var pageDefaultFile = setting.Page.DefaultFile.Trim('/');
            var folderPath = setting.Page.FolderPath.IsNullOrEmpty() ? "Document" : setting.Page.FolderPath;

            var docPath = Path.IsPathRooted(folderPath) ? folderPath : Path.Combine(AppContext.BaseDirectory, folderPath);
            if (!Directory.Exists(docPath)) docPath = Path.Combine(Directory.GetCurrentDirectory(), folderPath);
            var hasDocFolder = Directory.Exists(docPath);

            var pageEnabled = setting.Page.Enable && hasDocFolder;

            app.Use(async (context, next) =>
            {
                var path = context.Request.Path.Value;
                if (path != null && (path.Equals($"/{baseRoute}", StringComparison.OrdinalIgnoreCase) || path.Equals($"/{baseRoute}/", StringComparison.OrdinalIgnoreCase)))
                {
                    var targetRoute = pageEnabled ? pageDefaultFile : "swagger";
                    var target = $"{context.Request.PathBase}/{baseRoute}/{targetRoute}{context.Request.QueryString}";
                    context.Response.Redirect(target, permanent: false);
                    return;
                }

                await next();
            });

            if (pageEnabled)
            {
                app.UseFileServer(new FileServerOptions
                {
                    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(docPath),
                    RequestPath = new PathString($"/{baseRoute}"),
                    EnableDefaultFiles = true,
                });
            }

            if (setting.Swagger.Enable)
            {
                app.UseSwagger(options =>
                {
                    options.RouteTemplate = $"{baseRoute}/swagger/{{documentName}}/swagger.json";
                });

                app.UseReDoc(options =>
                {
                    options.RoutePrefix = $"{baseRoute}/swagger";
                    options.IndexStream = () =>
                    {
                        var assembly = AppDomain.CurrentDomain.GetAssemblies()
                            .First(x => x.GetName().Name == $"{nameof(TripleSix)}.{nameof(Core)}");
                        var streamName = assembly.GetManifestResourceNames()
                            .First(x => x.EndsWith("ReDoc.html"));
                        return assembly.GetManifestResourceStream(streamName);
                    };
                });
            }

            return app;
        }

        /// <summary>
        /// Cài đặt endpoint tài liệu hệ thống.
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/>.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        /// <returns><see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder UseDocument(
            this IApplicationBuilder app,
            IConfiguration configuration)
        {
            return UseDocument(app, new DocumentAppsetting(configuration));
        }

        /// <summary>
        /// Cài đặt OpenTelemetry.
        /// </summary>
        /// <param name="builder"><see cref="WebApplicationBuilder"/>.</param>
        /// <param name="setting"><see cref="OpenTelemetryAppsetting"/>.</param>
        public static void SetupOpenTelemetry(this WebApplicationBuilder builder, OpenTelemetryAppsetting setting)
        {
            if (setting.Enable == false) return;

            builder.Logging.AddOpenTelemetry(config =>
            {
                config.IncludeScopes = true;
                config.IncludeFormattedMessage = true;
                config.ParseStateValues = true;
                if (setting.AttachLog) config.AttachLogsToActivityEvent();
            });

            builder.Services.AddOpenTelemetry()
                .ConfigureResource(resource =>
                {
                    resource.AddService(setting.ServiceName!);
                })
                .WithTracing(tracing =>
                {
                    tracing.AddOtlpExporter(config =>
                    {
                        config.Endpoint = new Uri(setting.Host!);
                    });

                    tracing.AddAspNetCoreInstrumentation(o =>
                    {
                        o.Filter = (context) =>
                        {
                            return context.Request.Method != HttpMethods.Options;
                        };

                        o.EnrichWithHttpResponse = (activity, response) =>
                        {
                            activity.DisplayName = $"[API] {activity.DisplayName}";
                        };
                    });

                    tracing.AddSqlClientInstrumentation(o =>
                    {
                        o.Filter = cmd =>
                        {
                            if (cmd is SqlCommand sqlCommand)
                            {
                                var connectionString = sqlCommand.Connection?.ConnectionString ?? string.Empty;
                                var excludedDatabases = new[] { "Hangfire" };
                                foreach (var dbName in excludedDatabases)
                                {
                                    if (connectionString.Contains($"Database={dbName}", StringComparison.OrdinalIgnoreCase) ||
                                        connectionString.Contains($"Initial Catalog={dbName}", StringComparison.OrdinalIgnoreCase))
                                    {
                                        return false;
                                    }
                                }
                            }

                            return true;
                        };
                        o.EnrichWithSqlCommand = (activity, cmd) =>
                        {
                            activity.DisplayName = $"[DB] {activity.DisplayName}";
                        };
                    });

                    tracing.AddHttpClientInstrumentation(o =>
                    {
                        o.EnrichWithHttpRequestMessage = (activity, requestMessage) =>
                        {
                            if (requestMessage.RequestUri != null)
                            {
                                activity.DisplayName = $"[HTTP] {requestMessage.Method} {requestMessage.RequestUri.Authority}{NormalizeUrlPath(requestMessage.RequestUri.LocalPath)}";
                                activity.SetTag("peer.service", requestMessage.RequestUri.Authority);
                            }

                            try
                            {
                                // enrich callback là Action (không hỗ trợ async); chỉ lấy curl khi task
                                // hoàn thành sẵn (content dạng buffer), tránh async void & block thread
                                var curlTask = requestMessage.ToCurl();
                                if (curlTask.IsCompletedSuccessfully)
                                    activity.SetTag("http.curl", curlTask.Result);
                            }
                            catch
                            {
                            }
                        };
                    });
                });
        }

        /// <summary>
        /// Cài đặt OpenTelemetry.
        /// </summary>
        /// <param name="builder"><see cref="WebApplicationBuilder"/>.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        public static void SetupOpenTelemetry(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            SetupOpenTelemetry(builder, new OpenTelemetryAppsetting(configuration));
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

                var result = new ErrorResult(statusCode, errorCode, errorMessage).ToJsonText();
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
            app.UseResponseCompression();

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

            app.UseMiddleware<OpenTelemetryMiddleware>();
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

        // thay các segment high-cardinality (guid, số) bằng {id} để span name không bị phân mảnh
        private static string NormalizeUrlPath(string path)
        {
            if (path.IsNullOrEmpty())
            {
                return path;
            }

            var segments = path.Split('/');

            for (var i = 0; i < segments.Length; i++)
            {
                var segment = segments[i];

                if (segment.Length == 0)
                {
                    continue;
                }

                if (Guid.TryParse(segment, out _))
                {
                    segments[i] = "{id}";
                    continue;
                }

                var isAllDigits = true;

                for (var j = 0; j < segment.Length; j++)
                {
                    if (!char.IsDigit(segment[j]))
                    {
                        isAllDigits = false;
                        break;
                    }
                }

                if (isAllDigits)
                {
                    segments[i] = "{id}";
                }
            }

            return string.Join('/', segments);
        }
    }
}
