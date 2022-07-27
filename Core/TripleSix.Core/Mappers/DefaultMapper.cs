﻿using System.Reflection;
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
        public DefaultMapper(Assembly assembly)
        {
            var dtoTypes = assembly.GetExportedTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsAssignableTo<IDto>());
            var entityTypes = assembly.GetExportedTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsAssignableTo<IEntity>());

            foreach (var entityType in entityTypes)
            {
                var matchedDtoTypes = SelectDto(entityType, dtoTypes);
                foreach (var dtoType in matchedDtoTypes)
                {
                    CreateMap(entityType, dtoType, MemberList.None);
                    var mapToEntity = CreateMap(dtoType, entityType, MemberList.Destination);

                    var config = dtoType.GetCustomAttribute<MapEntityAttribute>();
                    if (config == null) continue;

                    if (config.IgnoreUnmapedProperties)
                    {
                        var unmapProperties = entityType
                            .GetPublicProperties()
                            .Where(x => dtoType.GetProperty(x.Name) == null);
                        foreach (var property in unmapProperties)
                            mapToEntity.ForMember(property.Name, o => o.Ignore());
                    }
                }
            }
        }

        private IEnumerable<Type> SelectDto(Type entityType, IEnumerable<Type> dtoTypes)
        {
            return dtoTypes.Where(x =>
            {
                var mapEntity = x.GetCustomAttribute<MapEntityAttribute>();
                return mapEntity != null && mapEntity.EntityType == entityType;
            });
        }
    }
}
