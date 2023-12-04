﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;
using TripleSix.Core.Enums;
using TripleSix.Core.Helpers;
using TripleSix.Core.WebApi.Filters;

namespace TripleSix.Core.AutoAdmin
{
    public class MethodMetadata
    {
        public MethodMetadata(ControllerMetadata controllerMetadata, MethodInfo methodType)
        {
            var controllerType = controllerMetadata.ControllerType;
            var controllerCode = controllerMetadata.ControllerCode;
            var controllerInfo = controllerType.GetCustomAttribute<AdminControllerAttribute>();
            var methodInfo = methodType.GetCustomAttribute<AdminMethodAttribute>();
            if (methodInfo.AdminType is null) methodInfo.AdminType = controllerInfo.AdminType;

            var route = controllerType.GetCustomAttributes().FirstOrDefault(x => x is RouteAttribute) as RouteAttribute;
            var httpMethod = methodType.GetCustomAttribute<HttpMethodAttribute>();
            Method = httpMethod.HttpMethods.First();
            Url = route is null ? "[controller]" : route.Template;
            if (!Url.StartsWith("/")) Url = "/" + Url;
            if (httpMethod.Template.IsNotNullOrWhiteSpace()) Url += "/" + httpMethod.Template;
            Url = Url.Replace("[controller]", controllerCode);

            MethodType = methodType;
            Type = methodInfo.Type;
            Controller = controllerMetadata.Code;

            Name = methodInfo.Name;
            if (Name.IsNullOrWhiteSpace())
            {
                switch (Type)
                {
                    case AdminMethodTypes.List:
                        Name = "danh sách [controller]";
                        break;
                    case AdminMethodTypes.Detail:
                        Name = "chi tiết [controller]";
                        break;
                    case AdminMethodTypes.Create:
                        Name = "tạo [controller]";
                        break;
                    case AdminMethodTypes.Update:
                        Name = "sửa [controller]";
                        break;
                    case AdminMethodTypes.Delete:
                        Name = "xóa [controller]";
                        break;
                    case AdminMethodTypes.Restore:
                        Name = "khôi phục [controller]";
                        break;
                    case AdminMethodTypes.ListChangeLog:
                        Name = "lịch sử thay đổi [controller]";
                        break;
                    case AdminMethodTypes.DetailChangeLog:
                        Name = "chi tiết thay đổi [controller]";
                        break;
                    case AdminMethodTypes.Export:
                        Name = "xuất [controller]";
                        break;
                }
            }

            Name = Name.Replace("[controller]", controllerMetadata.Name);

            var permission = methodType.GetCustomAttribute<PermissionRequirement>();
            if (permission is not null)
            {
                var groupCode = string.Empty;
                if (permission.AutoGroup)
                {
                    groupCode = controllerInfo.PermissionGroup.IsNullOrWhiteSpace()
                        ? controllerMetadata.Code.ToCamelCase() + "."
                        : controllerInfo.PermissionGroup + ".";
                }

                var listCodes = new List<string>();
                if (permission.Code.IsNotNullOrWhiteSpace())
                    listCodes.Add(groupCode + permission.Code);
                if (permission.ListCode.IsNotNullOrEmpty())
                {
                    listCodes.AddRange(permission.ListCode
                        .Where(x => x.IsNotNullOrWhiteSpace())
                        .Select(x => groupCode + x));
                }

                PermissionOperator = permission.Operator;
                PermissionCodes = listCodes.ToArray();
            }
        }

        public AdminMethodTypes Type { get; set; }

        public string Name { get; set; }

        public string Controller { get; set; }

        public string Method { get; set; }

        public string Url { get; set; }

        public PermissionOperators PermissionOperator { get; set; }

        public string[] PermissionCodes { get; set; }

        [JsonIgnore]
        public MethodInfo MethodType { get; set; }

        public static MethodInfo[] GetListMethodOfController(Type controllerType)
        {
            var controllerInfo = controllerType.GetCustomAttribute<AdminControllerAttribute>();
            var adminType = controllerInfo.AdminType;
            var entityType = adminType.GetEntityType();
            var filterType = adminType.GetNestedType("Filter");
            var itemType = adminType.GetNestedType("Item");
            var detailType = adminType.GetNestedType("Detail");
            var createType = adminType.GetNestedType("Create");
            var updateType = adminType.GetNestedType("Update");

            var autoControllers = new List<TypeInfo>();
            var exportedTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(x => x.IsPublic)
                .Where(x => !x.IsAbstract);

            if (controllerInfo.EnableRead && filterType is not null && itemType is not null && detailType is not null)
            {
                var type = exportedTypes.FirstOrDefault(t => t.IsSubclassOfRawGeneric(typeof(BaseAdminControllerReadMethod<,,,,>)));
                if (type is not null)
                    autoControllers.Add(type.MakeGenericType(adminType, entityType, filterType, itemType, detailType).GetTypeInfo());
            }

            if (controllerInfo.EnableCreate && createType is not null)
            {
                var type = exportedTypes.FirstOrDefault(t => t.IsSubclassOfRawGeneric(typeof(BaseAdminControllerCreateMethod<,,>)));
                if (type is not null)
                    autoControllers.Add(type.MakeGenericType(adminType, entityType, createType).GetTypeInfo());
            }

            if (controllerInfo.EnableUpdate && updateType is not null)
            {
                var type = exportedTypes.FirstOrDefault(t => t.IsSubclassOfRawGeneric(typeof(BaseAdminControllerUpdateMethod<,,>)));
                if (type is not null)
                    autoControllers.Add(type.MakeGenericType(adminType, entityType, updateType).GetTypeInfo());
            }

            if (controllerInfo.EnableDelete)
            {
                var type = exportedTypes.FirstOrDefault(t => t.IsSubclassOfRawGeneric(typeof(BaseAdminControllerDeleteMethod<,>)));
                if (type is not null)
                    autoControllers.Add(type.MakeGenericType(adminType, entityType).GetTypeInfo());
            }

            if (controllerInfo.EnableChangeLog)
            {
                var type = exportedTypes.FirstOrDefault(t => t.IsSubclassOfRawGeneric(typeof(BaseAdminControllerChangeLogMethod<,>)));
                if (type is not null)
                    autoControllers.Add(type.MakeGenericType(adminType, entityType).GetTypeInfo());
            }

            if (controllerInfo.EnableExport && filterType is not null && detailType is not null)
            {
                var type = exportedTypes.FirstOrDefault(t => t.IsSubclassOfRawGeneric(typeof(BaseAdminControllerExportMethod<,,,>)));
                if (type is not null)
                    autoControllers.Add(type.MakeGenericType(adminType, entityType, filterType, detailType).GetTypeInfo());
            }

            return autoControllers.SelectMany(x => x.GetMethods())
                .Where(x => x.IsPublic)
                .Where(x => !x.IsAbstract)
                .Where(x => x.GetCustomAttribute<HttpMethodAttribute>() is not null)
                .Where(x => x.GetCustomAttribute<AdminMethodAttribute>().Enable == true)
                .Where(x => EnumHelper.GetValues<AdminMethodTypes>().Contains(x.GetCustomAttribute<AdminMethodAttribute>().Type))
                .ToArray();
        }

        public static MethodMetadata GenerateMethodMetadata(ControllerMetadata controllerMetadata, MethodInfo methodType)
        {
            var methodInfo = methodType.GetCustomAttribute<AdminMethodAttribute>();

            switch (methodInfo.Type)
            {
                case AdminMethodTypes.List:
                    return new MethodListMetadata(controllerMetadata, methodType);

                case AdminMethodTypes.Detail:
                    return new MethodDetailMetadata(controllerMetadata, methodType);

                case AdminMethodTypes.Create:
                    return new MethodInputMetadata(controllerMetadata, methodType, "Create");

                case AdminMethodTypes.Update:
                    return new MethodInputMetadata(controllerMetadata, methodType, "Update");

                default:
                    return new MethodMetadata(controllerMetadata, methodType);
            }
        }
    }
}
