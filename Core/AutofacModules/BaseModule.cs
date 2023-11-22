using Autofac;
using Microsoft.Extensions.Configuration;

namespace TripleSix.Core.AutofacModules
{
    /// <summary>
    /// Base autofac module.
    /// </summary>
    public abstract class BaseModule : Module
    {
        /// <summary>
        /// Base autofac module.
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
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
