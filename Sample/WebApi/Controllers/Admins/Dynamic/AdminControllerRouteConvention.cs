using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Sample.WebApi.Controllers.Admins
{
    public class AdminControllerRouteConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerType.IsGenericType)
            {
                controller.Selectors.Add(new SelectorModel
                {
                    AttributeRouteModel = new AttributeRouteModel(new RouteAttribute("Admin/Account"))
                });
            }
        }
    }
}
