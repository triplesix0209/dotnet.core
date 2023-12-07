using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Controller endpoint feature provider.
    /// </summary>
    public class ControllerEndpointFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly Assembly _assembly;

        /// <summary>
        /// Controller endpoint feature provider.
        /// </summary>
        /// <param name="assembly"><see cref="Assembly"/>.</param>
        public ControllerEndpointFeatureProvider(Assembly assembly)
        {
            _assembly = assembly;
        }

        /// <inheritdoc/>
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            var controllerTypes = _assembly.GetExportedTypes()
                .Where(x => !x.IsAbstract && !x.IsInterface)
                .Where(x => x.IsSubclassOf(typeof(BaseController)));
            var endpointAttributeTypes = _assembly.GetExportedTypes()
                .Where(x => !x.IsAbstract && !x.IsInterface)
                .Where(x => x.IsAssignableTo<IControllerEndpointAttribute>());

            foreach (var controllerType in controllerTypes)
            {
                foreach (var endpointAttributeType in endpointAttributeTypes)
                {
                    if (controllerType.GetCustomAttribute(endpointAttributeType) is IControllerEndpointAttribute attr)
                        feature.Controllers.Add(attr.EndpointType.GetTypeInfo());
                }
            }
        }
    }
}
