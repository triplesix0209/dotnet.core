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
            var exportedTypes = _executingAssembly.GetExportedTypes().Where(x => !x.IsAbstract);
            var candidates = exportedTypes
                .Where(t =>
                {
                    var metadata = t.GetCustomAttribute<AdminControllerAttribute>();
                    if (metadata is null) return false;
                    return metadata.Enable && metadata.AdminType is not null && metadata.EntityType is not null;
                });

            foreach (var candidate in candidates)
            {
                var info = candidate.GetCustomAttribute<AdminControllerAttribute>();
                var entityType = info.EntityType;
                var filterType = info.AdminType.GetNestedType("Filter");
                var itemType = info.AdminType.GetNestedType("Item");
                var detailType = info.AdminType.GetNestedType("Detail");
                var createType = info.AdminType.GetNestedType("Create");
                var updateType = info.AdminType.GetNestedType("Update");

                if (info.EnableRead && filterType is not null && itemType is not null && detailType is not null)
                {
                    var type = exportedTypes.FirstOrDefault(t => t.IsSubclassOfRawGeneric(typeof(BaseAdminControllerReadMethod<,,,>)));
                    if (type is not null)
                        feature.Controllers.Add(type.MakeGenericType(entityType, filterType, itemType, detailType).GetTypeInfo());
                }

                if (info.EnableCreate && createType is not null)
                {
                    var type = exportedTypes.FirstOrDefault(t => t.IsSubclassOfRawGeneric(typeof(BaseAdminControllerCreateMethod<,>)));
                    if (type is not null)
                        feature.Controllers.Add(type.MakeGenericType(entityType, createType).GetTypeInfo());
                }

                if (info.EnableUpdate && updateType is not null)
                {
                    var type = exportedTypes.FirstOrDefault(t => t.IsSubclassOfRawGeneric(typeof(BaseAdminControllerUpdateMethod<,>)));
                    if (type is not null)
                        feature.Controllers.Add(type.MakeGenericType(entityType, updateType).GetTypeInfo());
                }

                if (info.EnableDelete)
                {
                    var type = exportedTypes.FirstOrDefault(t => t.IsSubclassOfRawGeneric(typeof(BaseAdminControllerDeleteMethod<>)));
                    if (type is not null)
                        feature.Controllers.Add(type.MakeGenericType(entityType).GetTypeInfo());
                }

                if (info.EnableChangeLog)
                {
                    var type = exportedTypes.FirstOrDefault(t => t.IsSubclassOfRawGeneric(typeof(BaseAdminControllerChangeLogMethod<>)));
                    if (type is not null)
                        feature.Controllers.Add(type.MakeGenericType(entityType).GetTypeInfo());
                }

                if (info.EnableExport && filterType is not null && detailType is not null)
                {
                    var type = exportedTypes.FirstOrDefault(t => t.IsSubclassOfRawGeneric(typeof(BaseAdminControllerExportMethod<,,>)));
                    if (type is not null)
                        feature.Controllers.Add(type.MakeGenericType(entityType, filterType, detailType).GetTypeInfo());
                }
            }
        }
    }
}
