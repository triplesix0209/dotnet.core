#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CpTech.Core.DataContexts;
using CpTech.Core.Events;
using CpTech.Core.Mappers;
using CpTech.Core.Repositories;
using CpTech.Core.Services;
using CpTech.Core.WebApi.Controllers;
using Microsoft.EntityFrameworkCore;

namespace CpTech.Core.ModuleAutofac
{
    public static class ModuleAutofacExtension
    {
        public static IRegistrationBuilder<IEventPublisher, ConcreteReflectionActivatorData, SingleRegistrationStyle>
            RegisterEventPublisher(this ContainerBuilder builder)
        {
            return builder.RegisterType<EventPublisher>()
                .InstancePerLifetimeScope()
                .As<IEventPublisher>();
        }

        public static IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> RegisterDbContext<T>(
            this ContainerBuilder builder,
            Func<IComponentContext, T> @delegate)
            where T : DbContext
        {
            return builder.Register(@delegate)
                .InstancePerLifetimeScope()
                .As<BaseDataContext>()
                .As<DbContext>()
                .AsSelf();
        }

        public static IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> RegisterRepository<T>(
            this ContainerBuilder builder,
            Func<IComponentContext, T> @delegate)
            where T : IRepository
        {
            return builder.Register(@delegate)
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

        public static IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> RegisterService<T>(
            this ContainerBuilder builder,
            Func<IComponentContext, T> @delegate)
            where T : IService
        {
            return builder.Register(@delegate)
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces()
                .AsSelf();
        }

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterAllService(
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

        public static void RegisterMapper(this ContainerBuilder builder, Assembly assembly, params Profile[] mappers)
        {
            builder.Register(c => new MapperConfiguration(config =>
                {
                    config.AddExpressionMapping();
                    config.Advanced.AllowAdditiveTypeMapCreation = true;

                    foreach (var mapper in mappers)
                    {
                        config.AddProfile(mapper);
                    }
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

        public static void RegisterAllMapper(this ContainerBuilder builder, Assembly assembly)
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
                config.Advanced.AllowAdditiveTypeMapCreation = true;

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