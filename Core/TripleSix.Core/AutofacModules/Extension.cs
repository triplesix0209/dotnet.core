using System.Reflection;
using Autofac;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using AutoMapper.Internal;
using TripleSix.Core.Mappers;
using TripleSix.Core.Repositories.Interfaces;
using TripleSix.Core.Services.Interfaces;
using TripleSix.Core.WebApi.Controllers;

namespace TripleSix.Core.AutofacModules
{
    public static class Extension
    {
        public static void RegisterAllMapper(this ContainerBuilder builder, Assembly assembly)
        {
            builder.RegisterAssemblyTypes(assembly)
                .PublicOnly()
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsAssignableTo<BaseMapper>())
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .SingleInstance()
                .AsSelf();

            builder.Register(c => new MapperConfiguration(config =>
            {
                config.AddExpressionMapping();
                config.Internal().AllowAdditiveTypeMapCreation = true;

                var mappers = assembly.GetExportedTypes()
                    .Where(x => !x.IsAbstract)
                    .Where(x => x.IsAssignableTo<BaseMapper>())
                    .ToList();
                mappers.Sort((a, b) =>
                {
                    //if (a.IsAssignableTo<GlobalMapper>()) return -1;
                    //else if (b.IsAssignableTo<GlobalMapper>()) return 1;
                    return 0;
                });

                config.AddProfiles(mappers.Select(t => c.Resolve(t) as Profile));
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

        public static void RegisterAllRepository(this ContainerBuilder builder, Assembly assembly)
        {
            builder.RegisterAssemblyTypes(assembly)
                .PublicOnly()
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsAssignableTo<IRepository>())
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces();
        }

        public static void RegisterAllService(this ContainerBuilder builder, Assembly assembly)
        {
            builder.RegisterAssemblyTypes(assembly)
                .PublicOnly()
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsAssignableTo<IService>())
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces();
        }

        public static void RegisterAllController(this ContainerBuilder builder, Assembly assembly)
        {
            builder.RegisterAssemblyTypes(assembly)
                .PublicOnly()
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsAssignableTo<BaseController>())
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
        }
    }
}