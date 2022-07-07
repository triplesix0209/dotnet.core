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
        /// Configuration từ appsetting.
        /// </summary>
        protected IConfiguration Configuration { get; }
    }
}
