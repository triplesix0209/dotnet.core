﻿using System;
using System.Linq;
using System.Reflection;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.AutoAdmin
{
    public class ModelMetadata
    {
        public ModelMetadata(Type controllerType, MethodInfo methodType, PropertyInfo fieldType, Type modelType)
        {
            if (modelType is null || !modelType.IsAssignableTo<IAdminDto>()) return;

            var modelControllerType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => !t.IsAbstract)
                .Where(t => t.GetCustomAttribute<AdminControllerAttribute>()?.Enable == true)
                .Where(t => t.GetCustomAttribute<AdminControllerAttribute>()?.AdminType == modelType)
                .FirstOrDefault();
            var modelControllerName = modelControllerType.Name.Substring(0, controllerType.Name.LastIndexOf("Controller"));

            Code = modelControllerName.ToKebabCase();
        }

        public string Code { get; set; }
    }
}