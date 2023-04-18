using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.AutoAdmin
{
    public class AutoAdminControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly Assembly _assembly;

        public AutoAdminControllerFeatureProvider(Assembly assembly)
        {
            _assembly = assembly;
        }

        public virtual void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            var exportedTypes = _assembly.GetExportedTypes().Where(x => !x.IsAbstract);
            var candidates = exportedTypes.Where(t =>
            {
                var metadata = t.GetCustomAttribute<AdminControllerAttribute>();
                if (metadata is null) return false;
                return metadata.Enable && metadata.ModelType is not null;
            });

            foreach (var candidate in candidates)
            {
                var info = candidate.GetCustomAttribute<AdminControllerAttribute>();
                if (info == null) continue;

                var adminType = info.ModelType;
                if (!adminType.IsSubclassOfRawGeneric(typeof(AdminModel<>))) continue;
                var entityType = AdminModel.GetEntityType(adminType);
                if (entityType == null) continue;

                var filterType = adminType.GetNestedType("Filter");
                var itemType = adminType.GetNestedType("Item");
                var detailType = adminType.GetNestedType("Detail");
                var createType = adminType.GetNestedType("Create");
                var updateType = adminType.GetNestedType("Update");

                if (info.EnableRead && filterType is not null && itemType is not null && detailType is not null)
                {
                    var type = exportedTypes.FirstOrDefault(t => t.IsSubclassOfRawGeneric(typeof(BaseAdminControllerReadMethod<,,,,>)));
                    if (type is not null)
                    {
                        feature.Controllers.Add(type.MakeGenericType(entityType, adminType, filterType, itemType, detailType)
                            .GetTypeInfo());
                    }
                }

                if (info.EnableCreate && createType is not null)
                {
                    var type = exportedTypes.FirstOrDefault(t => t.IsSubclassOfRawGeneric(typeof(BaseAdminControllerCreateMethod<,,>)));
                    if (type is not null)
                    {
                        feature.Controllers.Add(type.MakeGenericType(entityType, adminType, createType)
                            .GetTypeInfo());
                    }
                }

                if (info.EnableDelete && updateType is not null)
                {
                    var type = exportedTypes.FirstOrDefault(t => t.IsSubclassOfRawGeneric(typeof(BaseAdminControllerUpdateMethod<,,>)));
                    if (type is not null)
                    {
                        feature.Controllers.Add(type.MakeGenericType(entityType, adminType, updateType)
                            .GetTypeInfo());
                    }
                }

                if (info.EnableDelete)
                {
                    var type = exportedTypes.FirstOrDefault(t => t.IsSubclassOfRawGeneric(typeof(BaseAdminControllerDeleteMethod<,>)));
                    if (type is not null)
                    {
                        feature.Controllers.Add(type.MakeGenericType(entityType, adminType)
                            .GetTypeInfo());
                    }
                }

                if (info.EnableExport && filterType is not null && detailType is not null)
                {
                    var type = exportedTypes.FirstOrDefault(t => t.IsSubclassOfRawGeneric(typeof(BaseAdminControllerExportMethod<,,,>)));
                    if (type is not null)
                    {
                        feature.Controllers.Add(type.MakeGenericType(entityType, adminType, filterType, detailType)
                            .GetTypeInfo());
                    }
                }

                if (info.EnableChangeLog)
                {
                    var type = exportedTypes.FirstOrDefault(t => t.IsSubclassOfRawGeneric(typeof(BaseAdminControllerChangeLogMethod<,>)));
                    if (type is not null)
                    {
                        feature.Controllers.Add(type.MakeGenericType(entityType, adminType)
                            .GetTypeInfo());
                    }
                }
            }
        }
    }
}
