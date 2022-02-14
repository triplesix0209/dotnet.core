using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TripleSix.Core.DataTypes;
using TripleSix.Core.Dto;
using TripleSix.Core.Entities;

namespace TripleSix.Core.Mappers
{
    public abstract class GlobalMapper : BaseMapper
    {
        protected GlobalMapper()
        {
            CreateProfile();
            CreateMap<string, Phone>().ConvertUsing(s => new Phone(s));
            CreateMap<Phone, string>().ConvertUsing(s => s == null ? null : s.ToString());
        }

        protected virtual IEnumerable<Type> SelectEntityType()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes()
                .Where(t => t.IsPublic)
                .Where(t => typeof(IEntity).IsAssignableFrom(t))
                .Where(t => t.Name.EndsWith("Entity")));
        }

        protected abstract IEnumerable<Type> SelectDtoType(string objectName);

        private void CreateProfile()
        {
            var entityTypes = SelectEntityType();
            foreach (var entityType in entityTypes)
            {
                var objectName = entityType.Name.Substring(0, entityType.Name.IndexOf("Entity", StringComparison.Ordinal));

                CreateMapToEntity(entityType, entityType);
                CreateMapToEntity(typeof(ModelDataDto), entityType, MemberList.None);

                var dtoTypes = SelectDtoType(objectName);
                foreach (var dtoType in dtoTypes)
                {
                    CreateMap(typeof(ModelDataDto), dtoType, MemberList.None).ReverseMap();
                    CreateMapToEntity(dtoType, entityType, MemberList.None).ReverseMap();
                }
            }
        }
    }
}