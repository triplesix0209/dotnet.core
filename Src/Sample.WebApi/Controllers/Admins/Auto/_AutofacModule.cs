#pragma warning disable SA1649 // File name should match first type name

using Autofac;
using Microsoft.Extensions.Configuration;
using TripleSix.CoreOld.ModuleAutofac;

namespace Sample.WebApi.Controllers.Admins.Auto
{
    public class AutofacModule : BaseModule
    {
        public AutofacModule(IConfiguration configuration)
            : base(configuration)
        {
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterGeneric(typeof(AdminControllerReadMethod<,,,,>))
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterGeneric(typeof(AdminControllerCreateMethod<,,>))
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterGeneric(typeof(AdminControllerUpdateMethod<,,>))
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterGeneric(typeof(AdminControllerDeleteMethod<,>))
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterGeneric(typeof(AdminControllerChangeLogMethod<,>))
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterGeneric(typeof(AdminControllerExportMethod<,,,>))
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
        }
    }
}
