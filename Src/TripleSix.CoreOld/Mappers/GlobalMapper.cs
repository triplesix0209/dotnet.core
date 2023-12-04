using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TripleSix.CoreOld.AutoAdmin;
using TripleSix.CoreOld.DataTypes;
using TripleSix.CoreOld.Dto;
using TripleSix.CoreOld.Entities;
using TripleSix.CoreOld.Helpers;

namespace TripleSix.CoreOld.Mappers
{
    public abstract class GlobalMapper : BaseMapper
    {
        protected GlobalMapper(string[] excludeAssemblyNames = null)
        {
            CreateMap<string, Phone>().ConvertUsing(s => new Phone(s));
            CreateMap<Phone, string>().ConvertUsing(s => s == null ? null : s.ToString());
            CreateProfile(excludeAssemblyNames);
        }

        protected virtual IEnumerable<Type> SelectEntityType(string[] excludeAssemblyNames = null)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => excludeAssemblyNames == null || !excludeAssemblyNames.Contains(assembly.GetName().Name))
                .SelectMany(assembly => assembly.GetTypes()
                .Where(t => t.IsPublic)
                .Where(t => t.IsAssignableTo<IEntity>())
                .Where(t => t.Name.EndsWith("Entity")));
        }

        protected abstract IEnumerable<Type> SelectDtoType(string objectName);

        protected virtual void CreateProfile(string[] excludeAssemblyNames = null)
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
                .Where(assembly => excludeAssemblyNames == null || !excludeAssemblyNames.Contains(assembly.GetName().Name))
                .SelectMany(assembly => assembly.GetTypes()
                .Where(t => t.IsPublic))
                .Where(t => !t.IsAbstract)
                .Where(t => t.IsAssignableTo<IAdminDto>());
            foreach (var adminDto in adminDtos)
            {
                var entityType = adminDto.GetEntityType(excludeAssemblyNames);
                if (entityType is null) continue;
                CreateMapAdmin(entityType, adminDto);
            }
        }
    }
}
