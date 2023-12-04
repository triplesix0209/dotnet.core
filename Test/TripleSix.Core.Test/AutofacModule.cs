using Autofac;
using Microsoft.Extensions.Configuration;
using TripleSix.CoreOld.ModuleAutofac;

namespace TripleSix.CoreOld.Test
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
