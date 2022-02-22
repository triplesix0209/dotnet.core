using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.AutoAdmin
{
    public class MethodMetadata
    {
        public MethodMetadata(Type controllerType, MethodInfo methodType)
        {
            var controllerInfo = controllerType.GetCustomAttribute<AdminControllerAttribute>();
            var controllerName = controllerType.Name.Substring(0, controllerType.Name.LastIndexOf("Controller"));
            var methodInfo = methodType.GetCustomAttribute<AdminMethodAttribute>();
            if (methodInfo.EntityType is null) methodInfo.EntityType = controllerInfo.EntityType;
            if (methodInfo.AdminType is null) methodInfo.AdminType = controllerInfo.AdminType;

            var route = controllerType.GetCustomAttributes().FirstOrDefault(x => x is RouteAttribute) as RouteAttribute;
            var httpMethod = methodType.GetCustomAttribute<HttpMethodAttribute>();
            Method = httpMethod.HttpMethods.First();
            Url = route is null ? "[controller]" : route.Template;
            if (!Url.StartsWith("/")) Url = "/" + Url;
            if (httpMethod.Template.IsNotNullOrWhiteSpace()) Url += "/" + httpMethod.Template;
            Url = Url.Replace("[controller]", controllerName);

            Type = methodInfo.Type;
        }

        public AdminMethodTypes Type { get; set; }

        public string Method { get; set; }

        public string Url { get; set; }

        public static MethodInfo[] GetListMethodOfController(Type controllerType)
        {
            var controllerInfo = controllerType.GetCustomAttribute<AdminControllerAttribute>();
            var entityType = controllerInfo.EntityType;
            var filterType = controllerInfo.AdminType.GetNestedType("Filter");
            var itemType = controllerInfo.AdminType.GetNestedType("Item");
            var detailType = controllerInfo.AdminType.GetNestedType("Detail");
            var createType = controllerInfo.AdminType.GetNestedType("Create");
            var updateType = controllerInfo.AdminType.GetNestedType("Update");

            var autoControllers = new List<TypeInfo>();
            var exportedTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(x => x.IsPublic)
                .Where(x => !x.IsAbstract);

            if (controllerInfo.EnableRead && filterType is not null && itemType is not null && detailType is not null)
            {
                var type = exportedTypes.FirstOrDefault(t => t.IsSubclassOfRawGeneric(typeof(BaseAdminControllerReadMethod<,,,>)));
                if (type is not null)
                    autoControllers.Add(type.MakeGenericType(entityType, filterType, itemType, detailType).GetTypeInfo());
            }

            if (controllerInfo.EnableCreate && createType is not null)
            {
                var type = exportedTypes.FirstOrDefault(t => t.IsSubclassOfRawGeneric(typeof(BaseAdminControllerCreateMethod<,>)));
                if (type is not null)
                    autoControllers.Add(type.MakeGenericType(entityType, createType).GetTypeInfo());
            }

            if (controllerInfo.EnableUpdate && updateType is not null)
            {
                var type = exportedTypes.FirstOrDefault(t => t.IsSubclassOfRawGeneric(typeof(BaseAdminControllerUpdateMethod<,>)));
                if (type is not null)
                    autoControllers.Add(type.MakeGenericType(entityType, updateType).GetTypeInfo());
            }

            if (controllerInfo.EnableDelete)
            {
                var type = exportedTypes.FirstOrDefault(t => t.IsSubclassOfRawGeneric(typeof(BaseAdminControllerDeleteMethod<>)));
                if (type is not null)
                    autoControllers.Add(type.MakeGenericType(entityType).GetTypeInfo());
            }

            if (controllerInfo.EnableChangeLog)
            {
                var type = exportedTypes.FirstOrDefault(t => t.IsSubclassOfRawGeneric(typeof(BaseAdminControllerChangeLogMethod<>)));
                if (type is not null)
                    autoControllers.Add(type.MakeGenericType(entityType).GetTypeInfo());
            }

            if (controllerInfo.EnableExport && filterType is not null && detailType is not null)
            {
                var type = exportedTypes.FirstOrDefault(t => t.IsSubclassOfRawGeneric(typeof(BaseAdminControllerExportMethod<,,>)));
                if (type is not null)
                    autoControllers.Add(type.MakeGenericType(entityType, filterType, detailType).GetTypeInfo());
            }

            return autoControllers.SelectMany(x => x.GetMethods())
                .Where(x => x.IsPublic)
                .Where(x => !x.IsAbstract)
                .Where(x => x.GetCustomAttribute<HttpMethodAttribute>() is not null)
                .Where(x => x.GetCustomAttribute<AdminMethodAttribute>().Enable == true)
                .Where(x => EnumHelper.GetValues<AdminMethodTypes>().Contains(x.GetCustomAttribute<AdminMethodAttribute>().Type))
                .ToArray();
        }

        public static MethodMetadata GenerateMethodMetadata(Type controllerType, MethodInfo methodType)
        {
            var methodInfo = methodType.GetCustomAttribute<AdminMethodAttribute>();

            switch (methodInfo.Type)
            {
                case AdminMethodTypes.List:
                    return new MethodListMetadata(controllerType, methodType);

                case AdminMethodTypes.Detail:
                    return new MethodDetailMetadata(controllerType, methodType);

                case AdminMethodTypes.Create:
                    return new MethodInputMetadata(controllerType, methodType, "Create");

                case AdminMethodTypes.Update:
                    return new MethodInputMetadata(controllerType, methodType, "Update");

                default:
                    return new MethodMetadata(controllerType, methodType);
            }
        }
    }
}
