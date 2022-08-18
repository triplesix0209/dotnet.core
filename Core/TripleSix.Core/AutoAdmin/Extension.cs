using System.Reflection;
using Autofac;
using Autofac.Extras.DynamicProxy;
using TripleSix.Core.AutofacModules;

namespace TripleSix.Core.AutoAdmin
{
    public static class Extension
    {
        /// <summary>
        /// Đăng ký tất cả các admin method.
        /// </summary>
        /// <param name="builder">Container builder.</param>
        /// <param name="assembly">Assembly chứa các IAdminMethod.</param>
        public static void RegisterAllAdminMethod(
            this ContainerBuilder builder,
            Assembly assembly)
        {
            var autoAdminTypes = assembly.GetExportedTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsAssignableTo<IAdminMethod>());
            foreach (var autoAdminType in autoAdminTypes)
            {
                builder.RegisterGeneric(autoAdminType)
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            }
        }

        /// <summary>
        /// Đăng ký các thành phần phục vụ chức năng Object Log.
        /// </summary>
        /// <typeparam name="TObjectLogService">Service impelemnt <see cref="IObjectLogService"/>.</typeparam>
        /// <param name="builder">Container builder.</param>
        public static void RegisterObjectLog<TObjectLogService>(
            this ContainerBuilder builder)
            where TObjectLogService : IObjectLogService, new()
        {
            builder.Register(c => new TObjectLogService())
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ServiceInterceptor))
                .As<IObjectLogService>();
        }
    }
}