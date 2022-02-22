using System;
using System.Linq;
using System.Reflection;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.AutoAdmin
{
    public class MethodInputMetadata : MethodMetadata
    {
        public MethodInputMetadata(Type controllerType, MethodInfo methodType, string nestedTypeForInput)
            : base(controllerType, methodType)
        {
            var controllerInfo = controllerType.GetCustomAttribute<AdminControllerAttribute>();
            var methodInfo = methodType.GetCustomAttribute<AdminMethodAttribute>();
            var adminType = methodInfo.AdminType ?? controllerInfo.AdminType;
            var inputType = adminType.GetNestedType(nestedTypeForInput);

            InputFields = inputType.GetProperties()
                .OrderBy(x => x.DeclaringType.BaseTypesAndSelf().Count())
                .Select(fieldType => new FieldInputMetadata(controllerType, methodType, fieldType))
                .ToArray();
        }

        public FieldInputMetadata[] InputFields { get; set; }
    }
}
