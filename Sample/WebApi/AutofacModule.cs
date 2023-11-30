using Autofac;
using TripleSix.Core.AutofacModules;

namespace Sample.WebApi
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

            builder.RegisterAllQuartzJob(assembly);
            builder.RegisterAllController(assembly);
            builder.RegisterHangfireStartup(assembly);
        }
    }
}
