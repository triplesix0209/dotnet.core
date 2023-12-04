using Autofac;
using Microsoft.Extensions.Configuration;
using TripleSix.CoreOld.ModuleAutofac;

namespace TripleSix.CoreOld.AutoAdmin
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

            builder.RegisterRepository<ObjectLogRepository>();
        }
    }
}