using System.Reflection;
using Autofac;
using Microsoft.Extensions.Configuration;
using Sample.Infrastructure.Persistences;
using TripleSix.Core.AutofacModules;

namespace Sample.Infrastructure
{
    public class AutofacModule : BaseModule
    {
        private readonly Assembly _assembly;

        public AutofacModule(IConfiguration configuration, Assembly assembly)
            : base(configuration)
        {
            _assembly = assembly;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(c => new ApplicationDbContext(_assembly, Configuration))
                .PropertiesAutowired()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
