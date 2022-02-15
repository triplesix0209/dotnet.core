using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.AutoAdmin
{
    public class AdminControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly Assembly _executingAssembly;

        public AdminControllerFeatureProvider(Assembly executingAssembly)
        {
            _executingAssembly = executingAssembly;
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            var exportedTypes = _executingAssembly.GetExportedTypes();
            var candidates = exportedTypes
                .Where(t =>
                {
                    var metadata = t.GetCustomAttribute<AdminControllerAttribute>();
                    if (metadata is null) return false;
                    return metadata.Enable && metadata.AdminType is not null && metadata.EntityType is not null;
                });

            foreach (var candidate in candidates)
            {
                var metadata = candidate.GetCustomAttribute<AdminControllerAttribute>();
                var entityType = metadata.EntityType;
                var adminTypes = metadata.AdminType.GetNestedTypes();
                var filterType = adminTypes.FirstOrDefault(x => x.Name == "Filter");
                var itemType = adminTypes.FirstOrDefault(x => x.Name == "Item");
                var detailType = adminTypes.FirstOrDefault(x => x.Name == "Detail");
                var createType = adminTypes.FirstOrDefault(x => x.Name == "Create");
                var updateType = adminTypes.FirstOrDefault(x => x.Name == "Update");

                if (metadata.EnableRead && filterType is not null && itemType is not null && detailType is not null)
                {
                    var controllerType = exportedTypes.FirstOrDefault(
                        t => t.IsSubclassOfRawGeneric(typeof(BaseAdminControllerReadMethod<,,,>))
                        && !t.IsAbstract);
                    if (controllerType is not null)
                    {
                        feature.Controllers.Add(
                            controllerType.MakeGenericType(entityType, filterType, itemType, detailType)
                            .GetTypeInfo());
                    }
                }
            }
        }
    }
}
