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

            var domainAssembly = Assembly.Load(assembly.GetReferencedAssemblies().First(x => x.Name == $"{nameof(Sample)}.{nameof(Sample.Domain)}"));
            builder.RegisterAllMapper(assembly, domainAssembly);
            builder.RegisterAllRepository(assembly);
            builder.RegisterAllService(assembly);
        }
    }
}
