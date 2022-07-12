using Autofac;
using AutoMapper;
using TripleSix.Core.Entities;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Mappers
{
    /// <summary>
    /// Mapper profile cơ bản.
    /// </summary>
    public abstract class BaseMapper : Profile
    {
        public new IMappingExpression CreateMap(Type sourceType, Type destinationType)
        {
            return base.CreateMap(sourceType, destinationType);
        }

        public new IMappingExpression CreateMap(Type sourceType, Type destinationType, MemberList memberList)
        {
            if (sourceType.IsAssignableTo<IEntity>() && !destinationType.IsAssignableTo<IEntity>())
                return CreateMapFromEntity(sourceType, destinationType, memberList);
            if (!sourceType.IsAssignableTo<IEntity>() && destinationType.IsAssignableTo<IEntity>())
                return CreateMapToEntity(sourceType, destinationType, memberList);
            return base.CreateMap(sourceType, destinationType, memberList);
        }

        public new IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>()
        {
            return CreateMap<TSource, TDestination>(MemberList.Destination);
        }

        public new IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>(MemberList memberList)
        {
            var sourceType = typeof(TSource);
            var destinationType = typeof(TDestination);

            if (sourceType.IsAssignableTo<IEntity>() && !destinationType.IsAssignableTo<IEntity>())
                return CreateMapFromEntity<TSource, TDestination>(memberList);
            if (!sourceType.IsAssignableTo<IEntity>() && destinationType.IsAssignableTo<IEntity>())
                return CreateMapToEntity<TSource, TDestination>(memberList);
            return base.CreateMap<TSource, TDestination>(memberList);
        }

        #region [private methods]

        private IMappingExpression CreateMapFromEntity(Type entityType, Type destinationType, MemberList memberList)
        {
            return base.CreateMap(entityType, destinationType, memberList);
        }

        private IMappingExpression<TSource, TDestination> CreateMapFromEntity<TSource, TDestination>(MemberList memberList)
        {
            return base.CreateMap<TSource, TDestination>(memberList);
        }

        private IMappingExpression CreateMapToEntity(Type sourceType, Type entityType, MemberList memberList)
        {
            var map = base.CreateMap(sourceType, entityType, memberList);

            // bỏ qua các property reference
            var ignoreProperties = entityType
                .GetProperties()
                .Where(x =>
                {
                    var propertyType = x.PropertyType.GetUnderlyingType();
                    return propertyType.IsAssignableTo<IEntity>()
                        || propertyType.IsSubclassOfRawGeneric(typeof(ICollection<>));
                });
            foreach (var property in ignoreProperties)
                map.ForMember(property.Name, o => o.Ignore());

            // không map các property không có từ source
            if (entityType.IsAssignableTo<IStrongEntity>())
            {
                var unmapProperties = typeof(IStrongEntity)
                    .GetPublicProperties()
                    .Where(x => sourceType.GetProperty(x.Name) == null);
                foreach (var property in unmapProperties)
                    map.ForMember(property.Name, o => o.Ignore());
            }

            return map;
        }

        private IMappingExpression<TSource, TDestination> CreateMapToEntity<TSource, TDestination>(MemberList memberList)
        {
            var map = base.CreateMap<TSource, TDestination>(memberList);
            var sourceType = typeof(TSource);
            var entityType = typeof(TDestination);

            // bỏ qua các property reference
            var ignoreProperties = entityType
                .GetProperties()
                .Where(x =>
                {
                    var propertyType = x.PropertyType.GetUnderlyingType();
                    return propertyType.IsAssignableTo<IEntity>()
                        || propertyType.IsSubclassOfRawGeneric(typeof(ICollection<>));
                });
            foreach (var property in ignoreProperties)
                map.ForMember(property.Name, o => o.Ignore());

            // không map các property không có từ source
            if (entityType.IsAssignableTo<IStrongEntity>())
            {
                var unmapProperties = typeof(IStrongEntity)
                    .GetPublicProperties()
                    .Where(x => sourceType.GetProperty(x.Name) == null);
                foreach (var property in unmapProperties)
                    map.ForMember(property.Name, o => o.Ignore());
            }

            return map;
        }

        #endregion
    }
}
