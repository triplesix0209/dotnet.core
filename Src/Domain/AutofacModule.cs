using Autofac;
using Microsoft.Extensions.Configuration;
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
        }
    }
}
