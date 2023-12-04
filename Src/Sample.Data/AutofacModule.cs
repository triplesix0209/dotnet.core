using System.Reflection;
using Sample.Data.DataContexts;

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
