using System.Reflection;
using Autofac;
using Microsoft.Extensions.Configuration;
using TripleSix.Core.AutofacModules;

namespace Sample.Application
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
            var assembly = Assembly.GetExecutingAssembly();

            builder.RegisterAllMapper(assembly);
            builder.RegisterAllRepository(assembly);
            builder.RegisterAllService(assembly);
        }
    }
}
