using System;
using System.Linq;
using System.Reflection;
using TripleSix.CoreOld.Helpers;

namespace TripleSix.CoreOld.AutoAdmin
{
    public class ModelMetadata
    {
        public ModelMetadata(ControllerMetadata controllerMetadata, MethodMetadata methodMetadata, FieldMetadata fieldMetadata, Type modelType)
        {
            if (modelType is null || !modelType.IsAssignableTo<IAdminDto>()) throw new Exception($"\"{modelType.Name}\" is invalid for modelType");

            var modelControllerType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => !t.IsAbstract)
                .Where(t => t.GetCustomAttribute<AdminControllerAttribute>()?.Enable == true)
                .Where(t => t.GetCustomAttribute<AdminControllerAttribute>()?.AdminType == modelType)
                .FirstOrDefault();
            var modelControllerName = modelControllerType.Name.Substring(0, modelControllerType.Name.LastIndexOf("Controller"));

            Code = modelControllerName.ToKebabCase();
        }

        public string Code { get; set; }
    }
}
