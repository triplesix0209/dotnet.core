using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TripleSix.Core.ModuleAutofac;

namespace TripleSix.Identity.Common
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

            builder.RegisterEventPublisher();

            builder.RegisterType<LoggerFactory>()
                .As<ILoggerFactory>()
                .SingleInstance();

            builder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>))
                .SingleInstance();
        }
    }
}
