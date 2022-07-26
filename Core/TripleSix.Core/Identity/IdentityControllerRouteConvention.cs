using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace TripleSix.Core.Identity
{
    public class IdentityControllerRouteConvention
        : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (!controller.ControllerType.IsGenericType) return;
        }
    }
}
