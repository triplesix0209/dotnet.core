using System.Reflection;
using Autofac;
using AutoMapper;
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
        public DefaultMapper(Assembly entityAssembly, Assembly dtoAssembly)
        {
            var entityTypes = entityAssembly.GetExportedTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsAssignableTo<IEntity>());
            var dtoTypes = dtoAssembly.GetExportedTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsAssignableTo<IDto>());

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
                        if (!configToEntity.UnmapedProperties.IsNullOrEmpty())
                        {
                            var unmapProperties = entityType.GetPublicProperties()
                                .Where(x => configToEntity.UnmapedProperties.Contains(x.Name));
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
