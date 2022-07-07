using Autofac;
using Microsoft.Extensions.Configuration;

namespace TripleSix.Core.AutofacModules
{
    public abstract class BaseModule : Module
    {
        public BaseModule(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected IConfiguration Configuration { get; }
    }
}
