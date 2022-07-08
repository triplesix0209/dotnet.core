using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;
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
        /// <summary>
        /// Đăng ký các mapper dưới dạng InstancePerLifetimeScope với IMapper.
        /// </summary>
        /// <param name="builder">Container builder.</param>
        /// <param name="assembly">Assembly chứa các mapper để scan.</param>
        /// <returns>Registration builder cho phép tiếp tục cấu hình.</returns>
        public static IRegistrationBuilder<IMapper, SimpleActivatorData, SingleRegistrationStyle> RegisterAllMapper(this ContainerBuilder builder, Assembly assembly)
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

            return builder.Register(c =>
            {
                var context = c.Resolve<IComponentContext>();
                var config = context.Resolve<MapperConfiguration>();
                return config.CreateMapper(context.Resolve);
            })
                .InstancePerLifetimeScope()
                .As<IMapper>();
        }

        /// <summary>
        /// Đăng ký các repository dưới dạng InstancePerLifetimeScope với class khai báo.
        /// </summary>
        /// <param name="builder">Container builder.</param>
        /// <param name="assembly">Assembly chứa các repository để scan.</param>
        /// <returns>Registration builder cho phép tiếp tục cấu hình.</returns>
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterAllRepository(this ContainerBuilder builder, Assembly assembly)
        {
            return builder.RegisterAssemblyTypes(assembly)
                .PublicOnly()
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsAssignableTo<IRepository>())
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces()
                .AsSelf();
        }

        /// <summary>
        /// Đăng ký các service dưới dạng InstancePerLifetimeScope.
        /// </summary>
        /// <param name="builder">Container builder.</param>
        /// <param name="assembly">Assembly chứa các service để scan.</param>
        /// <returns>Registration builder cho phép tiếp tục cấu hình.</returns>
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterAllService(this ContainerBuilder builder, Assembly assembly)
        {
            return builder.RegisterAssemblyTypes(assembly)
                .PublicOnly()
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsAssignableTo<IService>())
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces();
        }

        /// <summary>
        /// Đăng ký các controller dưới dạng InstancePerLifetimeScope.
        /// </summary>
        /// <param name="builder">Container builder.</param>
        /// <param name="assembly">Assembly chứa các controller để scan.</param>
        /// <returns>Registration builder cho phép tiếp tục cấu hình.</returns>
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterAllController(this ContainerBuilder builder, Assembly assembly)
        {
            return builder.RegisterAssemblyTypes(assembly)
                .PublicOnly()
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsAssignableTo<BaseController>())
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .InstancePerLifetimeScope();
        }
    }
}