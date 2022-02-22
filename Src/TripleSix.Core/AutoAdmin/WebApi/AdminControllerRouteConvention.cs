using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using TripleSix.Core.Entities;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.AutoAdmin
{
    public class AdminControllerRouteConvention : IControllerModelConvention
    {
        private readonly Assembly _executingAssembly;

        public AdminControllerRouteConvention(Assembly executingAssembly)
        {
            _executingAssembly = executingAssembly;
        }

        public void Apply(ControllerModel controller)
        {
            if (!controller.ControllerType.IsGenericType) return;

            var entityType = controller.ControllerType.GetGenericArguments()[0];
            if (!entityType.IsAssignableTo<IModelEntity>()) return;

            var controllerBase = _executingAssembly.GetExportedTypes()
               .FirstOrDefault(t =>
               {
                   var metadata = t.GetCustomAttribute<AdminControllerAttribute>();
                   if (metadata is null) return false;
                   return metadata.AdminType is not null && metadata.EntityType == entityType;
               });
            if (controllerBase is null) return;

            controller.ControllerName = controllerBase.Name.Substring(0, controllerBase.Name.IndexOf("Controller"));
        }
    }
}
