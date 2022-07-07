using System.Reflection;
using Autofac;
using Microsoft.Extensions.Configuration;
using Sample.Infrastructure.Persistences;
using TripleSix.Core.AutofacModules;

namespace Sample.Infrastructure
{
    public class AutofacModule : BaseModule
    {
        private readonly Assembly _migrationAssembly;

        public AutofacModule(IConfiguration configuration, Assembly migrationAssembly)
            : base(configuration)
        {
            _migrationAssembly = migrationAssembly;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(c => new ApplicationDbContext(_migrationAssembly, Configuration))
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces();
        }
    }
}
