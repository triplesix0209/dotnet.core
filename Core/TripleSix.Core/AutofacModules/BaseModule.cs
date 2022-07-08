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

        /// <summary>
        /// Represents a set of key/value application configuration properties.
        /// </summary>
        protected IConfiguration Configuration { get; }
    }
}
