using System.Reflection;
using Autofac;
using AutoMapper;
using TripleSix.Core.Entities;
using TripleSix.Core.Helpers;
using TripleSix.Core.Mappers;

namespace TripleSix.Core.AutoAdmin
{
    /// <summary>
    /// Mapper profile auto admin.
    /// </summary>
    internal class AutoAdminMapper : BaseMapper
    {
        public AutoAdminMapper(Assembly assembly)
        {
            var entityTypes = assembly.GetExportedTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsAssignableTo<IEntity>());
            var adminTypes = assembly.GetExportedTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsSubclassOfRawGeneric(typeof(AdminModel<>)));

            foreach (var entityType in entityTypes)
            {
                var matchedAdminTypes = adminTypes
                    .Where(x => AdminModel.GetEntityType(x) == entityType);
                foreach (var adminType in matchedAdminTypes)
                {
                    var itemDto = adminType.GetNestedType("Item");
                    var detailDto = adminType.GetNestedType("Detail");
                    var createDto = adminType.GetNestedType("Create");
                    var updateDto = adminType.GetNestedType("Update");

                    if (itemDto != null)
                        CreateMap(entityType, itemDto, MemberList.None);

                    if (detailDto != null)
                        CreateMap(entityType, detailDto, MemberList.None);

                    if (createDto != null)
                    {
                        var map = CreateMap(createDto, entityType, MemberList.Destination);
                        var configToEntity = createDto.GetCustomAttribute<MapToEntityAttribute>();
                        if (configToEntity != null && configToEntity.IgnoreUnmapedProperties)
                        {
                            var unmapProperties = entityType.GetPublicProperties()
                                .Where(x => createDto.GetProperty(x.Name) == null);
                            foreach (var property in unmapProperties)
                                map.ForMember(property.Name, o => o.Ignore());
                        }
                    }

                    if (updateDto != null)
                    {
                        CreateMap(entityType, updateDto, MemberList.None);
                        var map = CreateMap(updateDto, entityType, MemberList.Destination);
                        var configToEntity = updateDto.GetCustomAttribute<MapToEntityAttribute>();
                        if (configToEntity != null && configToEntity.IgnoreUnmapedProperties)
                        {
                            var unmapProperties = entityType.GetPublicProperties()
                                .Where(x => updateDto.GetProperty(x.Name) == null);
                            foreach (var property in unmapProperties)
                                map.ForMember(property.Name, o => o.Ignore());
                        }
                    }
                }
            }

            CreateMap<ObjectLog, ChangeLogItemDto>(MemberList.Destination)
                .ForMember(d => d.Actor, o => o.MapFrom(s => s.CreatorId.HasValue ? new ActorDto { Id = s.CreatorId.Value } : null));

            CreateMap<ObjectLog, ChangeLogDetailDto>(MemberList.Destination)
                .IncludeBase<ObjectLog, ChangeLogItemDto>()
                .ForMember(d => d.ListField, o => o.MapFrom(s => s.Fields.Select(x => new ChangeLogField
                {
                    FieldName = x.FieldName,
                    OldValue = x.OldValue,
                    NewValue = x.NewValue,
                }).ToArray()));
        }
    }
}
