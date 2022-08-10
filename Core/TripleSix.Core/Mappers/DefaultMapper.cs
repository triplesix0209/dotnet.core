using System.Reflection;
using Autofac;
using AutoMapper;
using TripleSix.Core.AutoAdmin;
using TripleSix.Core.Entities;
using TripleSix.Core.Helpers;
using TripleSix.Core.Types;

namespace TripleSix.Core.Mappers
{
    /// <summary>
    /// Mapper profile mặc định.
    /// </summary>
    internal class DefaultMapper : BaseMapper
    {
        public DefaultMapper(Assembly assembly)
        {
            var entityTypes = assembly.GetExportedTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsAssignableTo<IEntity>());
            var dtoTypes = assembly.GetExportedTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsAssignableTo<IDto>());
            var adminTypes = assembly.GetExportedTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsSubclassOfRawGeneric(typeof(AdminModel<>)));

            foreach (var entityType in entityTypes)
            {
                var matchedDtoTypes = SelectDto(entityType, dtoTypes);
                foreach (var dtoType in matchedDtoTypes)
                {
                    var configFromEntity = dtoType.GetCustomAttribute<MapFromEntityAttribute>();
                    if (configFromEntity != null) CreateMap(entityType, dtoType, MemberList.None);

                    var configToEntity = dtoType.GetCustomAttribute<MapToEntityAttribute>();
                    if (configToEntity != null)
                    {
                        var map = CreateMap(dtoType, entityType, MemberList.Destination);
                        if (configToEntity.IgnoreUnmapedProperties)
                        {
                            var unmapProperties = entityType.GetPublicProperties()
                                .Where(x => dtoType.GetProperty(x.Name) == null);
                            foreach (var property in unmapProperties)
                                map.ForMember(property.Name, o => o.Ignore());
                        }
                    }
                }

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
        }

        private IEnumerable<Type> SelectDto(Type entityType, IEnumerable<Type> dtoTypes)
        {
            return dtoTypes.Where(x =>
            {
                var mapFromEntity = x.GetCustomAttribute<MapFromEntityAttribute>();
                var mapToEntity = x.GetCustomAttribute<MapToEntityAttribute>();
                return (mapFromEntity != null && mapFromEntity.EntityType == entityType)
                    || (mapToEntity != null && mapToEntity.EntityType == entityType);
            });
        }
    }
}
