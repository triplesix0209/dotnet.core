using System.Reflection;
using Autofac;
using Microsoft.Extensions.Configuration;
using TripleSix.CoreOld.ModuleAutofac;

namespace Sample.Quartz
{
    public class AutofacModule : BaseModule
    {
        private readonly Assembly _assembly = Assembly.GetExecutingAssembly();

        public AutofacModule(IConfiguration configuration)
            : base(configuration)
        {
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAllJob(_assembly);
        }
    }
}
