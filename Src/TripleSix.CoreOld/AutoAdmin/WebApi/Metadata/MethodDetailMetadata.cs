using System.Linq;
using System.Reflection;
using TripleSix.CoreOld.Dto;
using TripleSix.CoreOld.Helpers;

namespace TripleSix.CoreOld.AutoAdmin
{
    public class MethodDetailMetadata : MethodMetadata
    {
        public MethodDetailMetadata(ControllerMetadata controllerMetadata, MethodInfo methodType)
            : base(controllerMetadata, methodType)
        {
            var controllerType = controllerMetadata.ControllerType;
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
                .Select(fieldType => new FieldDisplayMetadata(controllerMetadata, this, fieldType))
                .ToArray();
            FieldDisplayMetadata.AfterProcess(DetailFields);

            DetailFieldGroups = DetailFields
                .Where(x => x.GroupData is not null)
                .GroupBy(x => x.GroupData.Code)
                .Select(x => x.First().GroupData)
                .ToArray();
        }

        public FieldDisplayMetadata[] DetailFields { get; set; }

        public GroupMetadata[] DetailFieldGroups { get; set; }
    }
}
