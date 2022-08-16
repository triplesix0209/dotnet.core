using System.Reflection;
using Autofac;

namespace TripleSix.Core.AutoAdmin
{
    public static class Extension
    {
        /// <summary>
        /// Đăng ký tất cả các admin method.
        /// </summary>
        /// <param name="builder">Container builder.</param>
        /// <param name="assembly">Assembly chứa các appsetting để scan.</param>
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
    }
}