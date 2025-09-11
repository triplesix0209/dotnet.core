using System.Linq.Expressions;
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
        public DefaultMapper(Assembly assembly)
        {
            MapFromEntityProfile(assembly);
            MapToEntityProfile(assembly);
        }

        private void MapFromEntityProfile(Assembly assembly)
        {
            var destinationTypes = assembly.GetExportedTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsAssignableToGenericType(typeof(IMapFromEntityDto<>)));
            foreach (var destinationType in destinationTypes)
            {
                var sourceType = destinationType.GetGenericArguments(typeof(IMapFromEntityDto<>))[0];

                // create map profile
                var createMap = GetType().GetMethods()
                    .First(x => x.DeclaringType == typeof(BaseMapper)
                        && x.Name == nameof(CreateMap) && x.IsGenericMethod
                        && x.GetParameters().Length > 0 && x.GetParameters()[0].ParameterType == typeof(MemberList))
                    .MakeGenericMethod(sourceType, destinationType);
                var mapper = createMap.Invoke(this, [MemberList.None]);
                if (mapper == null) continue;

                // ignore properties
                var ignoreProperties = destinationType.GetPublicProperties()
                    .Where(p => p.GetCustomAttribute<IgnoreMapFromEntityAttribute>() != null)
                    .Select(x => x.Name);
                foreach (var property in ignoreProperties.Distinct())
                {
                    var parameter = Expression.Parameter(
                        typeof(IMemberConfigurationExpression<,,>)
                        .MakeGenericType(sourceType, destinationType, typeof(object)));
                    var ignore = typeof(IProjectionMemberConfiguration<,,>)
                        .MakeGenericType(sourceType, destinationType, typeof(object))
                        .GetMethod(nameof(IProjectionMemberConfiguration<object, object, object>.Ignore));
                    var body = Expression.Call(parameter, ignore!);
                    var memberOptions = Expression.Lambda(body, parameter).Compile();

                    var forMember = mapper.GetType().GetMethods()
                        .First(x => x.Name == nameof(IMappingExpression.ForMember)
                            && x.GetParameters().Length == 2
                            && x.GetParameters()[0].ParameterType == typeof(string));
                    forMember?.Invoke(mapper, [property, memberOptions]);
                }
            }
        }

        private void MapToEntityProfile(Assembly assembly)
        {
            var sourceTypes = assembly.GetExportedTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsAssignableToGenericType(typeof(IMapToEntityDto<>)));
            foreach (var sourceType in sourceTypes)
            {
                var destinationType = sourceType.GetGenericArguments(typeof(IMapToEntityDto<>))[0];

                // create map profile
                var createMap = GetType().GetMethods()
                    .First(x => x.DeclaringType == typeof(BaseMapper)
                        && x.Name == nameof(CreateMap) && x.IsGenericMethod
                        && x.GetParameters().Length > 0 && x.GetParameters()[0].ParameterType == typeof(MemberList))
                    .MakeGenericMethod(sourceType, destinationType);
                var mapper = createMap.Invoke(this, [MemberList.Destination]);
                if (mapper == null) continue;

                // ignore properties
                var ignoreProperties = destinationType.GetPublicProperties()
                    .Where(p => sourceType.GetProperty(p.Name) == null || sourceType.GetProperty(p.Name)!.GetCustomAttribute<IgnoreMapToEntityAttribute>() != null)
                    .Select(p => p.Name)
                    .ToList();
                foreach (var property in ignoreProperties.Distinct())
                {
                    var parameter = Expression.Parameter(
                        typeof(IMemberConfigurationExpression<,,>)
                        .MakeGenericType(sourceType, destinationType, typeof(object)));
                    var ignore = typeof(IProjectionMemberConfiguration<,,>)
                        .MakeGenericType(sourceType, destinationType, typeof(object))
                        .GetMethod(nameof(IProjectionMemberConfiguration<object, object, object>.Ignore));
                    var body = Expression.Call(parameter, ignore!);
                    var memberOptions = Expression.Lambda(body, parameter).Compile();

                    var forMember = mapper.GetType().GetMethods()
                        .First(x => x.Name == nameof(IMappingExpression.ForMember)
                            && x.GetParameters().Length == 2
                            && x.GetParameters()[0].ParameterType == typeof(string));
                    forMember?.Invoke(mapper, [property, memberOptions]);
                }
            }
        }
    }
}
