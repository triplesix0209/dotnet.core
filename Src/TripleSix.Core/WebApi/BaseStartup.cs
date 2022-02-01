using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.ReDoc;
using Swashbuckle.AspNetCore.SwaggerGen;
using TripleSix.Core.DataTypes;
using TripleSix.Core.JsonSerializers.ContractResolvers;
using TripleSix.Core.JsonSerializers.Converters;
using TripleSix.Core.ModuleAutofac;
using TripleSix.Core.WebApi.ModelBinders;
using TripleSix.Core.WebApi.Swagger;

namespace TripleSix.Core.WebApi
{
    public class BaseStartup
    {
        private readonly Assembly _executingAssembly;

        protected BaseStartup(Assembly executingAssembly, IWebHostEnvironment env, IConfiguration configuration)
        {
            _executingAssembly = executingAssembly;
            Configuration = configuration;
        }

        public ILifetimeScope AutofacContainer { get; protected set; }

        public IConfiguration Configuration { get; protected set; }

        protected virtual BaseContractResolver BaseContractResolver { get => new BaseContractResolver(); }

        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAllController(_executingAssembly);
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddCors(ConfigureCors);
            services.AddAuthorization(ConfigureAuthorization);
            services.AddSwaggerGen(ConfigureSwagger).AddSwaggerGenNewtonsoftSupport();
            ConfigureMvc(services);
        }

        public virtual void ConfigureCors(CorsOptions options)
        {
        }

        public virtual void ConfigureAuthorization(AuthorizationOptions options)
        {
        }

        public virtual IMvcBuilder ConfigureMvc(
            IServiceCollection services,
            Action<MvcOptions> mvcAction = null,
            Action<MvcNewtonsoftJsonOptions> jsonAction = null)
        {
            var httpContextAccessor = new HttpContextAccessor();
            
            return services
                .AddSingleton<IHttpContextAccessor>(httpContextAccessor)
                .AddMvc(options =>
                {
                    options.ModelBinderProviders.Insert(0, new TimestampModelBinderProvider());
                    options.ModelBinderProviders.Insert(0, new PhoneModelBinderProvider());
                    options.AllowEmptyInputInBodyModelBinding = true;
                    if (mvcAction != null) mvcAction(options);
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = BaseContractResolver;
                    options.SerializerSettings.Converters.Add(new ModelConverter(httpContextAccessor, BaseContractResolver));
                    options.SerializerSettings.Converters.Add(new TimestampConverter());
                    options.SerializerSettings.Converters.Add(new PhoneConverter());
                    if (jsonAction != null) jsonAction(options);
                })
                .AddControllersAsServices();
        }

        public virtual void ConfigureSwagger(SwaggerGenOptions options)
        {
            options.SwaggerGeneratorOptions.DescribeAllParametersInCamelCase = true;
            options.CustomSchemaIds(x => x.FullName);
            options.EnableAnnotations();

            options.MapType<DateTime>(() => new OpenApiSchema { Type = "integer", Format = "int64" });
            options.MapType<DateTime?>(() => new OpenApiSchema { Type = "integer", Format = "int64", Nullable = true });
            options.MapType<Phone>(() => new OpenApiSchema { Type = "string", Nullable = true });

            options.DocumentFilter<BaseDocumentFilter>();
            options.OperationFilter<HidePropertyOperationFilter>();
            options.SchemaFilter<DescribeSchemaFilter>();
            options.SchemaFilter<EnumSchemaFilter>();
            options.SchemaFilter<HidePropertySchemaFilter>();
            options.ParameterFilter<DescribeParameterFilter>();
            options.ParameterFilter<EnumParameterFilter>();
        }

        public virtual void ConfigureReDoc(ReDocOptions options)
        {
            options.RoutePrefix = "swagger";
            options.IndexStream = () =>
            {
                var redocStream = GetType().GetTypeInfo().Assembly
                    .GetManifestResourceNames()
                    .First(x => x.EndsWith("ReDoc.html"));

                return GetType().GetTypeInfo().Assembly
                    .GetManifestResourceStream(redocStream);
            };
        }

        public virtual void BaseConfigure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            if (AutofacContainer.IsRegistered<MapperConfiguration>())
                AutofacContainer.Resolve<MapperConfiguration>().AssertConfigurationIsValid();

            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
