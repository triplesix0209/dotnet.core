using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace TripleSix.Core.AutoAdmin
{
    public class AdminControllerRouteConvention : IControllerModelConvention
    {
        private readonly Assembly _assembly;

        public AdminControllerRouteConvention(Assembly executingAssembly)
        {
            _assembly = executingAssembly;
        }

        public void Apply(ControllerModel controller)
        {
            if (!controller.ControllerType.IsGenericType) return;

            var adminModelType = controller.ControllerType.GetGenericArguments()[0];
            if (!adminModelType.IsAssignableTo<IAdminModel>()) return;

            var controllerBase = _assembly.GetExportedTypes()
               .FirstOrDefault(t => t.GetCustomAttribute<AdminControllerAttribute>()?.ModelType == adminModelType);
            if (controllerBase is null) return;

            controller.ControllerName = controllerBase.Name[..controllerBase.Name.IndexOf("Controller")];
        }
    }
}
