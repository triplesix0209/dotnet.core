using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Controller endpoint route convention.
    /// </summary>
    public class ControllerEndpointRouteConvention : IControllerModelConvention
    {
        /// <inheritdoc/>
        public void Apply(ControllerModel controller)
        {
            if (!controller.ControllerType.IsAssignableToGenericType(typeof(IControllerEndpoint<,>))) return;
            var genericArguments = controller.ControllerType.GetGenericArguments(typeof(IControllerEndpoint<,>));
            var controllerType = genericArguments[0];

            var controllerName = controllerType.Name;
            if (controllerName.ToLower().EndsWith("controller")) controllerName = controllerName[..^10];
            controller.ControllerName = controllerName;

            if (controllerType.GetCustomAttribute(genericArguments[1]) is IControllerEndpointAttribute endpointAttribute)
            {
                if (endpointAttribute.RequiredAnyScopes.IsNotNullOrEmpty())
                {
                    foreach (var action in controller.Actions)
                        action.Filters.Add(new RequireAnyScopeImplement(endpointAttribute.RequiredAnyScopes));
                }

                if (endpointAttribute.RequiredAnyIssuers.IsNotNullOrEmpty())
                {
                    foreach (var action in controller.Actions)
                        action.Filters.Add(new RequireAnyIssuerImplement(endpointAttribute.RequiredAnyIssuers));
                }
            }
        }
    }
}
