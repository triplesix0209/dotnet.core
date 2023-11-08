using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace TripleSix.Core.WebApi
{
    public class ControllerEndpointFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly Assembly _assembly;

        public ControllerEndpointFeatureProvider(Assembly assembly)
        {
            _assembly = assembly;
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            var controllerTypes = _assembly.GetExportedTypes()
                .Where(x => !x.IsAbstract && !x.IsInterface)
                .Where(x => x.IsSubclassOf(typeof(BaseController)));
            var endpointAttributeTypes = _assembly.GetExportedTypes()
                .Where(x => !x.IsAbstract && !x.IsInterface)
                .Where(x => x.IsSubclassOf(typeof(BaseControllerEndpointAttribute)));

            foreach (var controllerType in controllerTypes)
            {
                foreach (var endpointAttributeType in endpointAttributeTypes)
                {
                    if (controllerType.GetCustomAttribute(endpointAttributeType) is BaseControllerEndpointAttribute attr)
                        feature.Controllers.Add(attr.ToEndpointTypeInfo());
                }
            }
        }
    }
}
