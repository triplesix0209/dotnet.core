using System.Reflection;
using Autofac;
using Microsoft.Extensions.Configuration;
using Sample.Domain.Identity;
using TripleSix.Core.AutofacModules;

namespace Sample.Domain
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

            builder.RegisterIdentityContext<IdentityContext>();
            builder.RegisterAllAppsetting(assembly);
            builder.RegisterHangfireCore();
        }
    }
}
