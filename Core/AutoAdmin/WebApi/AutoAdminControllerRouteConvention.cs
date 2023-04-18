using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.AutoAdmin
{
    public class AutoAdminControllerRouteConvention : IControllerModelConvention
    {
        private readonly Assembly _assembly;

        public AutoAdminControllerRouteConvention(Assembly assembly)
        {
            _assembly = assembly;
        }

        public virtual void Apply(ControllerModel controller)
        {
            if (!controller.ControllerType.IsGenericType) return;

            var adminModelType = controller.ControllerType.GetGenericArguments()[1];
            if (!adminModelType.IsSubclassOfRawGeneric(typeof(AdminModel<>))) return;

            var controllerBase = _assembly.GetExportedTypes()
               .FirstOrDefault(t => t.GetCustomAttribute<AdminControllerAttribute>()?.ModelType == adminModelType);
            if (controllerBase is null) return;

            controller.ControllerName = controllerBase.Name[..controllerBase.Name.IndexOf("Controller")];
        }
    }
}
