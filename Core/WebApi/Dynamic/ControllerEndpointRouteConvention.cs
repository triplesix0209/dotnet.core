using Microsoft.AspNetCore.Mvc.ApplicationModels;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.WebApi
{
    public class ControllerEndpointRouteConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (!controller.ControllerType.IsAssignableToGenericType(typeof(IControllerEndpoint<>))) return;

            var controllerName = controller.ControllerType.GetGenericArguments(typeof(IControllerEndpoint<>))[0].Name;
            if (controllerName.ToLower().EndsWith("controller")) controllerName = controllerName[..^10];
            controller.ControllerName = controllerName;
        }
    }
}
