using System.Collections.Specialized;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Extras.DynamicProxy;
using Autofac.Extras.Quartz;
using Autofac.Features.Scanning;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Elastic.Clients.Elasticsearch;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TripleSix.Core.Appsettings;
using TripleSix.Core.DataContext;
using TripleSix.Core.Hangfire;
using TripleSix.Core.Helpers;
using TripleSix.Core.Identity;
using TripleSix.Core.Mappers;
using TripleSix.Core.Quartz;
using TripleSix.Core.Repositories;
using TripleSix.Core.Services;
using TripleSix.Core.WebApi;

namespace TripleSix.Core.AutofacModules
{
    /// <summary>
    /// Extension.
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// Đăng ký các auto injection dưới dạng InstancePerLifetimeScope.
        /// </summary>
        /// <param name="builder">Container builder.</param>
        /// <param name="assembly">Assembly chứa các service để scan.</param>
        /// <returns>Registration builder cho phép tiếp tục cấu hình.</returns>
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterAllAutoInjection(
            this ContainerBuilder builder,
            Assembly assembly)
        {
            return builder.RegisterAssemblyTypes(assembly)
                .PublicOnly()
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsAssignableTo<IAutoInjection>())
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces();
        }

        /// <summary>
        /// Đăng ký các Identity Context dưới dạng InstancePerLifetimeScope.
        /// </summary>
        /// <typeparam name="TIdentityContext">Class Identity Context sử dụng.</typeparam>
        /// <param name="builder">Container builder.</param>
        /// <returns>Registration builder cho phép tiếp tục cấu hình.</returns>
        public static IRegistrationBuilder<TIdentityContext, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterIdentityContext<TIdentityContext>(
            this ContainerBuilder builder)
            where TIdentityContext : IIdentityContext
        {
            return builder.RegisterType<TIdentityContext>()
                .WithParameter(new ResolvedParameter(
                    (p, c) => p.ParameterType == typeof(HttpContext),
                    (p, c) => c.Resolve<IHttpContextAccessor>().HttpContext!))
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces()
                .AsSelf();
        }

        /// <summary>
        /// Đăng ký các appsetting dưới dạng SingleInstance.
        /// </summary>
        /// <param name="builder">Container builder.</param>
        /// <param name="assembly">Assembly chứa các appsetting để scan.</param>
        /// <returns>Registration builder cho phép tiếp tục cấu hình.</returns>
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterAllAppsetting(
            this ContainerBuilder builder,
            Assembly assembly)
        {
            return builder.RegisterAssemblyTypes(assembly, Assembly.GetExecutingAssembly())
                .PublicOnly()
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsAssignableTo<BaseAppsetting>())
                .SingleInstance()
                .AsSelf();
        }

        /// <summary>
        /// Đăng ký các mapper dưới dạng InstancePerLifetimeScope với IMapper.
        /// </summary>
        /// <param name="builder">Container builder.</param>
        /// <param name="assembly">Assembly chứa các mapper và dto để scan.</param>
        /// <returns>Registration builder cho phép tiếp tục cấu hình.</returns>
        public static IRegistrationBuilder<IMapper, SimpleActivatorData, SingleRegistrationStyle> RegisterAllMapper(
            this ContainerBuilder builder,
            Assembly assembly)
        {
            builder.RegisterAssemblyTypes(assembly)
                .PublicOnly()
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsAssignableTo<BaseMapper>())
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .SingleInstance()
                .AsSelf();

            builder.RegisterAssemblyTypes(assembly)
                .PublicOnly()
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsAssignableToGenericType(typeof(IMappingAction<,>)))
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .InstancePerLifetimeScope()
                .AsSelf();

            builder.Register(c => new MapperConfiguration(config =>
            {
                config.AddExpressionMapping();

                config.AddProfile(new DefaultMapper(assembly));
                var mappers = assembly.GetExportedTypes()
                    .Where(x => !x.IsAbstract)
                    .Where(x => x.IsAssignableTo<BaseMapper>());
                config.AddProfiles(mappers.Select(t => c.Resolve(t) as Profile));
            })).SingleInstance()
                .AsSelf();

            return builder.Register(c =>
            {
                var context = c.Resolve<IComponentContext>();
                var config = context.Resolve<MapperConfiguration>();
                return config.CreateMapper(context.Resolve);
            }).InstancePerLifetimeScope()
                .As<IMapper>();
        }

        /// <summary>
        /// Đăng ký các repository dưới dạng InstancePerLifetimeScope với class khai báo.
        /// </summary>
        /// <param name="builder">Container builder.</param>
        /// <param name="assembly">Assembly chứa các repository để scan.</param>
        /// <returns>Registration builder cho phép tiếp tục cấu hình.</returns>
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterAllRepository(
            this ContainerBuilder builder,
            Assembly assembly)
        {
            return builder.RegisterAssemblyTypes(assembly)
                .PublicOnly()
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsAssignableTo<IRepository>())
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces();
        }

        /// <summary>
        /// Đăng ký các service dưới dạng InstancePerLifetimeScope.
        /// </summary>
        /// <param name="builder">Container builder.</param>
        /// <param name="assembly">Assembly chứa các service để scan.</param>
        /// <returns>Registration builder cho phép tiếp tục cấu hình.</returns>
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterAllService(
            this ContainerBuilder builder,
            Assembly assembly)
        {
            builder.Register(c => new ServiceInterceptor())
                .SingleInstance();

            return builder.RegisterAssemblyTypes(assembly)
                .PublicOnly()
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsAssignableTo<IService>())
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ServiceInterceptor))
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces();
        }

        /// <summary>
        /// Đăng ký các DbContext dưới dạng InstancePerLifetimeScope.
        /// </summary>
        /// <typeparam name="TDbContext">Kiểu DbContext.</typeparam>
        /// <param name="builder">Container builder.</param>
        /// <returns>Registration builder cho phép tiếp tục cấu hình.</returns>
        public static IRegistrationBuilder<TDbContext, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterDbContext<TDbContext>(
            this ContainerBuilder builder)
            where TDbContext : notnull, BaseDbContext
        {
            return builder.RegisterType<TDbContext>()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces();
        }

        /// <summary>
        /// Đăng ký tất cả các Quartz Job.
        /// </summary>
        /// <param name="builder">Container builder.</param>
        /// <param name="assembly">Assembly chứa các service để scan.</param>
        /// <param name="config">Danh sách config dạng key/value.</param>
        public static void RegisterAllQuartzJob(
            this ContainerBuilder builder,
            Assembly assembly,
            NameValueCollection? config = default)
        {
            builder.RegisterType<JobScheduler>()
               .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
               .AsSelf();

            builder.Register(_ => new ScopedDependency("global"))
                .AsImplementedInterfaces()
                .SingleInstance();

            if (config == null)
            {
                config = new NameValueCollection
                {
                    { "quartz.threadPool.threadCount", "3" },
                    { "quartz.scheduler.threadName", "Scheduler" },
                };
            }

            builder.RegisterModule(new QuartzAutofacFactoryModule
            {
                ConfigurationProvider = _ => config,
                JobScopeConfigurator = (builder, tag) =>
                {
                    builder.Register(_ => new ScopedDependency("job-" + DateTime.UtcNow.ToLongTimeString()))
                        .AsImplementedInterfaces()
                        .InstancePerMatchingLifetimeScope(tag);
                },
            });

            var jobTypes = assembly.GetTypes()
                .Where(t => t.IsPublic)
                .Where(t => !t.IsAbstract)
                .Where(t => t.IsAssignableTo<BaseJob>());
            foreach (var jobType in jobTypes)
                builder.RegisterModule(new QuartzAutofacJobsModule(jobType.Assembly) { AutoWireProperties = true });
        }

        /// <summary>
        /// Đăng ký các controller dưới dạng InstancePerLifetimeScope.
        /// </summary>
        /// <param name="builder">Container builder.</param>
        /// <param name="assembly">Assembly chứa các controller để scan.</param>
        /// <returns>Registration builder cho phép tiếp tục cấu hình.</returns>
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterAllController(
            this ContainerBuilder builder,
            Assembly assembly)
        {
            builder.RegisterAssemblyOpenGenericTypes(assembly)
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsAssignableToGenericType(typeof(IControllerEndpoint<,>)))
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            return builder.RegisterAssemblyTypes(assembly)
                .PublicOnly()
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsAssignableTo<BaseController>())
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .InstancePerLifetimeScope();
        }

        /// <summary>
        /// Đăng ký elastic client dưới dạng InstancePerLifetimeScope.
        /// </summary>
        /// <param name="builder">Container builder.</param>
        /// <returns>Registration builder cho phép tiếp tục cấu hình.</returns>
        public static IRegistrationBuilder<ElasticsearchClient, SimpleActivatorData, SingleRegistrationStyle> RegisterElasticClient(
            this ContainerBuilder builder)
        {
            return builder.Register(c =>
            {
                var config = c.Resolve<IConfiguration>();
                return Elastic.Extension.CreateElasticsearchClient(config);
            }).PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .InstancePerLifetimeScope()
                .AsSelf();
        }

        /// <summary>
        /// Đăng ký các core của hangfire.
        /// </summary>
        /// <param name="builder">Container builder.</param>
        public static void RegisterHangfireCore(
            this ContainerBuilder builder)
        {
            builder.Register(c => new HangfireExternalCaller())
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .InstancePerLifetimeScope();
        }

        /// <summary>
        /// Đăng ký startup của hangfire.
        /// </summary>
        /// <param name="builder">Container builder.</param>
        /// <param name="assembly">Assembly chứa các controller để scan.</param>
        /// <returns>Registration builder cho phép tiếp tục cấu hình.</returns>
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterHangfireStartup(
            this ContainerBuilder builder,
            Assembly assembly)
        {
            return builder.RegisterAssemblyTypes(assembly)
                .PublicOnly()
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsAssignableTo<HangfireBaseStartup>())
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .InstancePerLifetimeScope()
                .As<HangfireBaseStartup>();
        }
    }
}