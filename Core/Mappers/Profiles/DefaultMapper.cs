using System.Linq.Expressions;
using System.Reflection;
using Autofac;
using AutoMapper;
using TripleSix.Core.Entities;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Mappers
{
    /// <summary>
    /// Mapper profile mặc định.
    /// </summary>
    internal class DefaultMapper : BaseMapper
    {
        public DefaultMapper(Assembly entityAssembly, Assembly dtoAssembly)
        {
            var mapDataAttributes = dtoAssembly.GetExportedTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => x.GetCustomAttribute(typeof(MapDataAttribute<,>)) != null)
                .SelectMany(x => x.GetCustomAttributes(typeof(MapDataAttribute<,>)));
            foreach (var mapDataAttribute in mapDataAttributes)
            {
                // get source type & destination type
                var mapData = (Type)mapDataAttribute.TypeId;
                var mapTypes = mapData.GetGenericArguments();
                var sourceType = mapTypes[0];
                var destinationType = mapTypes[1];
                if (sourceType == null || destinationType == null) continue;

                // create map profile
                var createMap = GetType().GetMethods()
                    .First(x => x.DeclaringType == typeof(BaseMapper)
                        && x.Name == nameof(CreateMap) && x.IsGenericMethod
                        && x.GetParameters().Length > 0 && x.GetParameters()[0].ParameterType == typeof(MemberList))
                    .MakeGenericMethod(sourceType, destinationType);
                var isMapToEntity = destinationType.IsAssignableTo<IEntity>();
                var mapper = createMap.Invoke(this, new object[] { isMapToEntity ? MemberList.Destination : MemberList.None });
                if (mapper == null) continue;
                if (isMapToEntity)
                {
                    var ignoreProperties = new List<string>();

                    var ignoreUndeclareSourceProperty = mapData
                        .GetProperty(nameof(MapDataAttribute<object, object>.IgnoreUndeclareProperty))
                        ?.GetValue(mapDataAttribute) as bool?;
                    if (ignoreUndeclareSourceProperty == true)
                    {
                        ignoreProperties.AddRange(destinationType.GetPublicProperties()
                            .Where(p => sourceType.GetProperty(p.Name) == null)
                            .Select(p => p.Name));
                    }

                    var ignorePropertyAttributes = mapData
                        .GetProperty(nameof(MapDataAttribute<object, object>.IgnoreProperties))
                        ?.GetValue(mapDataAttribute) as string[];
                    if (ignorePropertyAttributes.IsNotNullOrEmpty())
                    {
                        ignoreProperties.AddRange(destinationType.GetPublicProperties()
                            .Where(p => ignorePropertyAttributes.Any(x => x == p.Name))
                            .Select(p => p.Name));
                    }

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
                        forMember?.Invoke(mapper, new object[] { property, memberOptions });
                    }
                }

                // create after map
                var mappingAction = mapTypes.Length > 2 ? mapTypes[2] : null;
                if (mappingAction != null)
                {
                    var afterMap = mapper.GetType().GetMethods()
                        .First(x => x.Name == nameof(IMappingExpression.AfterMap) && x.IsGenericMethod)
                        .MakeGenericMethod(mappingAction);
                    afterMap.Invoke(mapper, Array.Empty<object>());
                }
            }
        }
    }
}
