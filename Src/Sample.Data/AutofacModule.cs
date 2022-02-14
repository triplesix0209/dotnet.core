using System.Reflection;
using Autofac;
using Microsoft.Extensions.Configuration;
using Sample.Data.DataContexts;
using TripleSix.Core.ModuleAutofac;

namespace Sample.Data
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

            builder.RegisterDbContext<DataContext>();
            builder.RegisterAllRepository(_assembly);
        }
    }
}
