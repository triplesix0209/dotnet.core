using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sample.Common.Helpers;
using TripleSix.Core.ModuleAutofac;

namespace Sample.Common
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

            builder.Register(c => new MailHelper(Configuration))
               .PropertiesAutowired()
               .InstancePerLifetimeScope()
               .As<MailHelper>();

            builder.RegisterType<LoggerFactory>()
                .As<ILoggerFactory>()
                .SingleInstance();
            builder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>))
                .SingleInstance();
        }
    }
}
