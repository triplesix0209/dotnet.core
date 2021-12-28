using Autofac;
using Microsoft.Extensions.Configuration;

namespace TripleSix.Core.ModuleAutofac
{
    public class BaseModule : Module
    {
        public BaseModule(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
    }
}