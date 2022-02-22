using System;
using System.Linq;
using System.Reflection;
using TripleSix.Core.Dto;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.AutoAdmin
{
    public class MethodListMetadata : MethodMetadata
    {
        public MethodListMetadata(Type controllerType, MethodInfo methodType)
            : base(controllerType, methodType)
        {
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
                .Select(fieldType => new FieldInputMetadata(controllerType, methodType, fieldType))
                .ToArray();

            ItemFields = itemType.GetProperties()
                .Where(x => x.Name != nameof(IModelDataDto.Id))
                .Where(x => x.Name != nameof(IModelDataDto.IsDeleted))
                .Where(x => x.Name != nameof(IModelDataDto.CreatorId))
                .Where(x => x.Name != nameof(IModelDataDto.UpdaterId))
                .Where(x => x.Name != nameof(IModelDataDto.UpdateDatetime))
                .OrderBy(x => x.DeclaringType.BaseTypesAndSelf().Count())
                .Select(fieldType => new FieldItemMetadata(controllerType, methodType, fieldType))
                .ToArray();

            if (!ItemFields.Any(x => x.IsModelKey))
                ItemFields.First(x => x.Key == nameof(IModelDataDto.Code).ToCamelCase()).IsModelKey = true;

            if (!ItemFields.Any(x => x.IsModelText))
                ItemFields.First(x => x.Key == nameof(IModelDataDto.Code).ToCamelCase()).IsModelText = true;
        }

        public FieldInputMetadata[] FilterFields { get; set; }

        public FieldItemMetadata[] ItemFields { get; set; }
    }
}
