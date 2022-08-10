using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.AutoAdmin
{
    public class AdminControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly Assembly _assembly;

        public AdminControllerFeatureProvider(Assembly assembly)
        {
            _assembly = assembly;
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
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

                var adminModelType = info.ModelType;
                var entityType = adminModelType.GetCustomAttribute<AdminModelAttribute>()?.EntityType;
                if (entityType == null) continue;

                var filterType = adminModelType.GetNestedType("Filter");
                var itemType = adminModelType.GetNestedType("Item");
                var detailType = adminModelType.GetNestedType("Detail");
                var createType = adminModelType.GetNestedType("Create");
                var updateType = adminModelType.GetNestedType("Update");

                if (info.EnableRead && filterType is not null && itemType is not null && detailType is not null)
                {
                    var type = exportedTypes.FirstOrDefault(t => t.IsSubclassOfRawGeneric(typeof(BaseAdminControllerReadMethod<,,,,>)));
                    if (type is not null)
                    {
                        feature.Controllers.Add(type.MakeGenericType(
                            adminModelType,
                            entityType,
                            filterType,
                            itemType,
                            detailType)
                            .GetTypeInfo());
                    }
                }

                //if (info.EnableCreate && createType is not null)
                //{
                //    var type = exportedTypes.FirstOrDefault(t => t.IsSubclassOfRawGeneric(typeof(BaseAdminControllerCreateMethod<,,>)));
                //    if (type is not null)
                //        feature.Controllers.Add(type.MakeGenericType(adminType, entityType, createType).GetTypeInfo());
                //}

                //if (info.EnableUpdate && updateType is not null)
                //{
                //    var type = exportedTypes.FirstOrDefault(t => t.IsSubclassOfRawGeneric(typeof(BaseAdminControllerUpdateMethod<,,>)));
                //    if (type is not null)
                //        feature.Controllers.Add(type.MakeGenericType(adminType, entityType, updateType).GetTypeInfo());
                //}

                //if (info.EnableDelete)
                //{
                //    var type = exportedTypes.FirstOrDefault(t => t.IsSubclassOfRawGeneric(typeof(BaseAdminControllerDeleteMethod<,>)));
                //    if (type is not null)
                //        feature.Controllers.Add(type.MakeGenericType(adminType, entityType).GetTypeInfo());
                //}

                //if (info.EnableChangeLog)
                //{
                //    var type = exportedTypes.FirstOrDefault(t => t.IsSubclassOfRawGeneric(typeof(BaseAdminControllerChangeLogMethod<,>)));
                //    if (type is not null)
                //        feature.Controllers.Add(type.MakeGenericType(adminType, entityType).GetTypeInfo());
                //}

                //if (info.EnableExport && filterType is not null && detailType is not null)
                //{
                //    var type = exportedTypes.FirstOrDefault(t => t.IsSubclassOfRawGeneric(typeof(BaseAdminControllerExportMethod<,,,>)));
                //    if (type is not null)
                //        feature.Controllers.Add(type.MakeGenericType(adminType, entityType, filterType, detailType).GetTypeInfo());
                //}
            }
        }
    }
}
