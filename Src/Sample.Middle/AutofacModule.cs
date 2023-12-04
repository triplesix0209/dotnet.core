using System.Reflection;
using Sample.Middle.Helpers;

namespace Sample.Middle
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

            builder.RegisterAllMapper(_assembly);
            builder.RegisterAllService(_assembly);

            builder.Register(c => new MailHelper())
               .PropertiesAutowired()
               .InstancePerLifetimeScope()
               .As<MailHelper>();
        }
    }
}
