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
        public DefaultMapper(Assembly assembly)
        {
            MapFromEntityProfile(assembly);
            MapFromElasticDocumentProfile(assembly);
            MapToEntityProfile(assembly);
        }

        private void MapFromEntityProfile(Assembly assembly)
        {
            var destinationTypes = assembly.GetExportedTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => x.GetCustomAttribute(typeof(MapFromEntityAttribute<>)) != null);
            foreach (var destinationType in destinationTypes)
            {
                var mapAttributes = destinationType.GetCustomAttributes(typeof(MapFromEntityAttribute<>));
                foreach (var mapAttribute in mapAttributes)
                {
                    var mapTypes = mapAttribute.GetType().GetGenericArguments();
                    var sourceType = mapTypes[0];

                    // create map profile
                    var createMap = GetType().GetMethods()
                        .First(x => x.DeclaringType == typeof(BaseMapper)
                            && x.Name == nameof(CreateMap) && x.IsGenericMethod
                            && x.GetParameters().Length > 0 && x.GetParameters()[0].ParameterType == typeof(MemberList))
                        .MakeGenericMethod(sourceType, destinationType);
                    var mapper = createMap.Invoke(this, new object[] { MemberList.None });
                    if (mapper == null) continue;

                    // ignore properties
                    var ignoreProperties = new List<string>();
                    var ignoreSourceProperties = mapAttribute.GetType()
                        .GetProperty(nameof(MapToEntityAttribute<IEntity>.IgnoreProperties))
                        ?.GetValue(mapAttribute) as string[];
                    if (ignoreSourceProperties.IsNotNullOrEmpty())
                    {
                        ignoreProperties.AddRange(destinationType.GetPublicProperties()
                            .Where(p => ignoreSourceProperties.Any(x => x == p.Name))
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

                    // create after map
                    var mappingAction = mapTypes.Length > 1 ? mapTypes[1] : null;
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

        private void MapFromElasticDocumentProfile(Assembly assembly)
        {
            var destinationTypes = assembly.GetExportedTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => x.GetCustomAttribute(typeof(MapFromElasticDocumentAttribute<>)) != null);
            foreach (var destinationType in destinationTypes)
            {
                var mapAttributes = destinationType.GetCustomAttributes(typeof(MapFromElasticDocumentAttribute<>));
                foreach (var mapAttribute in mapAttributes)
                {
                    var mapTypes = mapAttribute.GetType().GetGenericArguments();
                    var sourceType = mapTypes[0];

                    // create map profile
                    var createMap = GetType().GetMethods()
                        .First(x => x.DeclaringType == typeof(BaseMapper)
                            && x.Name == nameof(CreateMap) && x.IsGenericMethod
                            && x.GetParameters().Length > 0 && x.GetParameters()[0].ParameterType == typeof(MemberList))
                        .MakeGenericMethod(sourceType, destinationType);
                    var mapper = createMap.Invoke(this, new object[] { MemberList.None });
                    if (mapper == null) continue;

                    // create after map
                    var mappingAction = mapTypes.Length > 1 ? mapTypes[1] : null;
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

        private void MapToEntityProfile(Assembly assembly)
        {
            var sourceTypes = assembly.GetExportedTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => x.GetCustomAttribute(typeof(MapToEntityAttribute<>)) != null);
            foreach (var sourceType in sourceTypes)
            {
                var mapAttributes = sourceType.GetCustomAttributes(typeof(MapToEntityAttribute<>));
                foreach (var mapAttribute in mapAttributes)
                {
                    var mapTypes = mapAttribute.GetType().GetGenericArguments();
                    var destinationType = mapTypes[0];

                    // create map profile
                    var createMap = GetType().GetMethods()
                        .First(x => x.DeclaringType == typeof(BaseMapper)
                            && x.Name == nameof(CreateMap) && x.IsGenericMethod
                            && x.GetParameters().Length > 0 && x.GetParameters()[0].ParameterType == typeof(MemberList))
                        .MakeGenericMethod(sourceType, destinationType);
                    var mapper = createMap.Invoke(this, new object[] { MemberList.Destination });
                    if (mapper == null) continue;

                    // ignore properties
                    var ignoreProperties = new List<string>();

                    var ignoreUndeclareProperty = mapAttribute.GetType()
                        .GetProperty(nameof(MapToEntityAttribute<IEntity>.IgnoreUndeclareProperty))
                        ?.GetValue(mapAttribute) as bool?;
                    if (ignoreUndeclareProperty == true)
                    {
                        ignoreProperties.AddRange(destinationType.GetPublicProperties()
                            .Where(p => sourceType.GetProperty(p.Name) == null)
                            .Select(p => p.Name));
                    }

                    var ignoreDestinationProperties = mapAttribute.GetType()
                        .GetProperty(nameof(MapToEntityAttribute<IEntity>.IgnoreProperties))
                        ?.GetValue(mapAttribute) as string[];
                    if (ignoreDestinationProperties.IsNotNullOrEmpty())
                    {
                        ignoreProperties.AddRange(destinationType.GetPublicProperties()
                            .Where(p => ignoreDestinationProperties.Any(x => x == p.Name))
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

                    // create after map
                    var mappingAction = mapTypes.Length > 1 ? mapTypes[1] : null;
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
}
