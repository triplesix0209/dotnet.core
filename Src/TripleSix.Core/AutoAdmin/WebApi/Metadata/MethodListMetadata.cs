using System.Linq;
using System.Reflection;
using TripleSix.Core.Dto;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.AutoAdmin
{
    public class MethodListMetadata : MethodMetadata
    {
        public MethodListMetadata(ControllerMetadata controllerMetadata, MethodInfo methodType)
            : base(controllerMetadata, methodType)
        {
            var controllerType = controllerMetadata.ControllerType;
            var controllerInfo = controllerType.GetCustomAttribute<AdminControllerAttribute>();
            var methodInfo = methodType.GetCustomAttribute<AdminMethodAttribute>();
            var adminType = methodInfo.AdminType ?? controllerInfo.AdminType;
            var filterType = adminType.GetNestedType("Filter");
            var itemType = adminType.GetNestedType("Item");

            FilterFields = filterType.GetProperties()
                .Where(x => x.Name != nameof(IPagingFilterDto.Page))
                .Where(x => x.Name != nameof(IPagingFilterDto.Size))
                .Where(x => x.Name != nameof(IModelFilterDto.SortColumn))
                .Where(x => x.Name != nameof(IModelFilterDto.Search))
                .Where(x => x.Name != nameof(IModelFilterDto.Id))
                .Where(x => x.Name != nameof(IModelFilterDto.CreatorId))
                .Where(x => x.Name != nameof(IModelFilterDto.UpdaterId))
                .OrderBy(x => x.DeclaringType.BaseTypesAndSelf().Count())
                .Select(fieldType => new FieldFilterMetadata(controllerMetadata, this, fieldType))
                .ToArray();

            ItemFields = itemType.GetProperties()
                .Where(x => x.Name != nameof(IModelDataDto.Id))
                .Where(x => x.Name != nameof(IModelDataDto.IsDeleted))
                .Where(x => x.Name != nameof(IModelDataDto.CreatorId))
                .Where(x => x.Name != nameof(IModelDataDto.UpdaterId))
                .Where(x => x.Name != nameof(IModelDataDto.UpdateDatetime))
                .OrderBy(x => x.DeclaringType.BaseTypesAndSelf().Count())
                .Select(fieldType => new FieldItemMetadata(controllerMetadata, this, fieldType))
                .ToArray();
            FieldDisplayMetadata.AfterProcess(ItemFields);
        }

        public FieldFilterMetadata[] FilterFields { get; set; }

        public FieldItemMetadata[] ItemFields { get; set; }
    }
}
