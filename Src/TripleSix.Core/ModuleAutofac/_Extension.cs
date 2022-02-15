#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Extras.Quartz;
using Autofac.Features.Scanning;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore;
using TripleSix.Core.DataContexts;
using TripleSix.Core.Mappers;
using TripleSix.Core.Quartz;
using TripleSix.Core.Repositories;
using TripleSix.Core.Services;
using TripleSix.Core.WebApi.Controllers;

namespace TripleSix.Core.ModuleAutofac
{
    public static class ModuleAutofacExtension
    {
        public static IRegistrationBuilder<T, ConcreteReflectionActivatorData, SingleRegistrationStyle>
            RegisterDbContext<T>(
                this ContainerBuilder builder)
                where T : DbContext
        {
            return builder.RegisterType<T>()
                .InstancePerLifetimeScope()
                .As<BaseDataContext>()
                .As<DbContext>()
                .AsSelf();
        }

        public static IRegistrationBuilder<T, ConcreteReflectionActivatorData, SingleRegistrationStyle>
            RegisterRepository<T>(
                this ContainerBuilder builder)
                where T : IRepository
        {
            return builder.RegisterType<T>()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces()
                .AsSelf();
        }

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterAllRepository(
                this ContainerBuilder builder,
                Assembly assembly)
        {
            return builder.RegisterAssemblyTypes(assembly)
                .PublicOnly()
                .Where(t => typeof(IRepository).IsAssignableFrom(t))
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces()
                .AsSelf();
        }

        public static IRegistrationBuilder<T, ConcreteReflectionActivatorData, SingleRegistrationStyle>
            RegisterService<T>(
                this ContainerBuilder builder)
                where T : IService
        {
            return builder.RegisterType<T>()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces()
                .AsSelf();
        }

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterAllService(
                this ContainerBuilder builder,
                Assembly assembly)
        {
            return builder.RegisterAssemblyTypes(assembly)
                .PublicOnly()
                .Where(t => typeof(IService).IsAssignableFrom(t))
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces()
                .AsSelf();
        }

        public static void RegisterMapper(
            this ContainerBuilder builder,
            params Profile[] mappers)
        {
            builder.Register(c => new MapperConfiguration(config =>
                {
                    config.AddExpressionMapping();

                    foreach (var mapper in mappers)
                        config.AddProfile(mapper);
                }))
                .SingleInstance()
                .AsSelf();

            builder.Register(c =>
            {
                var context = c.Resolve<IComponentContext>();
                var config = context.Resolve<MapperConfiguration>();
                return config.CreateMapper(context.Resolve);
            })
                .InstancePerLifetimeScope()
                .As<IMapper>();
        }

        public static void RegisterAllMapper(
            this ContainerBuilder builder,
            Assembly assembly)
        {
            builder.RegisterAssemblyTypes(assembly)
                .PublicOnly()
                .Where(t => typeof(BaseMapper).IsAssignableFrom(t))
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .InstancePerLifetimeScope()
                .AsSelf();

            builder.Register(c => new MapperConfiguration(config =>
            {
                config.AddExpressionMapping();

                var mappers = assembly.GetTypes()
                    .Where(t => t.IsClass && typeof(BaseMapper).IsAssignableFrom(t))
                    .ToList();

                mappers.Sort((a, b) =>
                {
                    if (typeof(GlobalMapper).IsAssignableFrom(a)) return -1;
                    else if (typeof(GlobalMapper).IsAssignableFrom(b)) return 1;
                    return 0;
                });

                var profiles = mappers.Select(t => c.Resolve(t) as Profile);
                config.AddProfiles(profiles);
            }))
                .SingleInstance()
                .AsSelf();

            builder.Register(c =>
            {
                var context = c.Resolve<IComponentContext>();
                var config = context.Resolve<MapperConfiguration>();
                return config.CreateMapper(context.Resolve);
            })
                .InstancePerLifetimeScope()
                .As<IMapper>();
        }

        public static void RegisterAllJob(
            this ContainerBuilder builder,
            Assembly assembly)
        {
            builder.RegisterType<JobScheduler>()
               .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
               .AsSelf();

            builder.Register(_ => new ScopedDependency("global"))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterModule(new QuartzAutofacFactoryModule
            {
                ConfigurationProvider = _ => new NameValueCollection
                {
                    { "quartz.threadPool.threadCount", "3" },
                    { "quartz.scheduler.threadName", "Scheduler" },
                },
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
                .Where(t => typeof(BaseJob).IsAssignableFrom(t));
            foreach (var jobType in jobTypes)
                builder.RegisterModule(new QuartzAutofacJobsModule(jobType.Assembly) { AutoWireProperties = true });
        }

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterAllController(this ContainerBuilder builder, Assembly assembly)
        {
            return builder.RegisterAssemblyTypes(assembly)
                .PublicOnly()
                .Where(t => typeof(BaseController).IsAssignableFrom(t))
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
        }
    }
}