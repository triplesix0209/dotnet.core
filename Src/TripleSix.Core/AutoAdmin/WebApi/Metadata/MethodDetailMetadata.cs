using System;
using System.Linq;
using System.Reflection;
using TripleSix.Core.Dto;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.AutoAdmin
{
    public class MethodDetailMetadata : MethodMetadata
    {
        public MethodDetailMetadata(Type controllerType, MethodInfo methodType)
            : base(controllerType, methodType)
        {
            var controllerInfo = controllerType.GetCustomAttribute<AdminControllerAttribute>();
            var methodInfo = methodType.GetCustomAttribute<AdminMethodAttribute>();
            var adminType = methodInfo.AdminType ?? controllerInfo.AdminType;
            var detailType = adminType.GetNestedType("Detail");

            DetailFields = detailType.GetProperties()
                .Where(x => x.Name != nameof(IModelDataDto.Id))
                .Where(x => x.Name != nameof(IModelDataDto.IsDeleted))
                .Where(x => x.Name != nameof(IModelDataDto.CreatorId))
                .Where(x => x.Name != nameof(IModelDataDto.UpdaterId))
                .Where(x => x.Name != nameof(IModelDataDto.CreateDatetime))
                .Where(x => x.Name != nameof(IModelDataDto.UpdateDatetime))
                .Where(x => x.GetCustomAttribute<AdminFieldAttribute>() is null || x.GetCustomAttribute<AdminFieldAttribute>().RenderOnDetail)
                .OrderBy(x => x.DeclaringType.BaseTypesAndSelf().Count())
                .Select(fieldType => new FieldDisplayMetadata(controllerType, methodType, fieldType))
                .ToArray();
            FieldDisplayMetadata.AfterProcess(DetailFields);
        }

        public FieldDisplayMetadata[] DetailFields { get; set; }
    }
}
