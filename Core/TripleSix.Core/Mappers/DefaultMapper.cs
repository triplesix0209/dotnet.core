using System.Reflection;
using Autofac;
using AutoMapper;
using TripleSix.Core.Entities;
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

            foreach (var entityType in entityTypes)
            {
                var matchedDtoTypes = SelectDto(entityType, dtoTypes);
                foreach (var dtoType in matchedDtoTypes)
                {
                    CreateMap(entityType, dtoType, MemberList.None);
                    CreateMap(dtoType, entityType, MemberList.Destination);
                }
            }
        }

        private IEnumerable<Type> SelectDto(Type entityType, IEnumerable<Type> dtoTypes)
        {
            return dtoTypes.Where(x => x.Name == entityType.Name + "Dto");
        }
    }
}
