using System.Linq;
using System.Reflection;
using TripleSix.CoreOld.Helpers;

namespace TripleSix.CoreOld.AutoAdmin
{
    public class MethodInputMetadata : MethodMetadata
    {
        public MethodInputMetadata(ControllerMetadata controllerMetadata, MethodInfo methodType, string nestedTypeForInput)
            : base(controllerMetadata, methodType)
        {
            var controllerType = controllerMetadata.ControllerType;
            var controllerInfo = controllerType.GetCustomAttribute<AdminControllerAttribute>();
            var methodInfo = methodType.GetCustomAttribute<AdminMethodAttribute>();
            var adminType = methodInfo.AdminType ?? controllerInfo.AdminType;
            var inputType = adminType.GetNestedType(nestedTypeForInput);

            InputFields = inputType.GetProperties()
                .OrderBy(x => x.DeclaringType.BaseTypesAndSelf().Count())
                .Select(fieldType => new FieldInputMetadata(controllerMetadata, this, fieldType))
                .ToArray();

            InputFieldGroups = InputFields
                .Where(x => x.GroupData is not null)
                .GroupBy(x => x.GroupData.Code)
                .Select(x => x.First().GroupData)
                .ToArray();
        }

        public FieldInputMetadata[] InputFields { get; set; }

        public GroupMetadata[] InputFieldGroups { get; set; }
    }
}
