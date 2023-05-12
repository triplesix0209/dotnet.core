using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TripleSix.Core.AutoAdmin;
using TripleSix.Core.DataTypes;
using TripleSix.Core.Dto;
using TripleSix.Core.Entities;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Mappers
{
    public abstract class GlobalMapper : BaseMapper
    {
        protected string[] excludeAssemblyNames = Array.Empty<string>();

        protected GlobalMapper()
        {
            CreateProfile();
            CreateMap<string, Phone>().ConvertUsing(s => new Phone(s));
            CreateMap<Phone, string>().ConvertUsing(s => s == null ? null : s.ToString());
        }

        protected virtual IEnumerable<Type> SelectEntityType()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => !excludeAssemblyNames.Contains(assembly.GetName().Name))
                .SelectMany(assembly => assembly.GetTypes()
                .Where(t => t.IsPublic)
                .Where(t => t.IsAssignableTo<IEntity>())
                .Where(t => t.Name.EndsWith("Entity")));
        }

        protected abstract IEnumerable<Type> SelectDtoType(string objectName);

        private void CreateProfile()
        {
            foreach (var entityType in SelectEntityType())
            {
                var objectName = entityType.Name.Substring(0, entityType.Name.IndexOf("Entity", StringComparison.Ordinal));

                CreateMapToEntity(entityType, entityType);
                CreateMapToEntity(typeof(ModelDataDto), entityType, MemberList.None);

                foreach (var dtoType in SelectDtoType(objectName))
                    CreateMapDto(entityType, dtoType);
            }

            var adminDtos = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(assembly => !excludeAssemblyNames.Contains(assembly.GetName().Name))
                    .SelectMany(assembly => assembly.GetTypes()
                    .Where(t => t.IsPublic))
                    .Where(t => !t.IsAbstract)
                    .Where(t => t.IsAssignableTo<IAdminDto>());
            foreach (var adminDto in adminDtos)
            {
                var entityType = adminDto.GetEntityType();
                if (entityType is null) continue;
                CreateMapAdmin(entityType, adminDto);
            }
        }
    }
}
