﻿using Autofac;
using AutoMapper;
using TripleSix.Core.Entities;
using TripleSix.Core.Helpers;
using TripleSix.Core.Types;

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

            // xử lý khi chỉ map các property có thay đổi của Data DTO
            if (sourceType.IsAssignableTo<IDto>())
            {
                foreach (var property in entityType.GetProperties())
                {
                    map.ForMember(property.Name, opt =>
                    {
                        opt.Condition((
                            source,
                            desc,
                            sourceVal,
                            descVal,
                            context) =>
                        {
                            if (!context.Items.ContainsKey("mapPropertyChangedOnly")) return true;
                            var mode = (string)context.Items["mapPropertyChangedOnly"];
                            if (mode == null) return true;

                            return mode.Trim() == "true" && ((IDto)source).IsPropertyChanged(property.Name);
                        });
                    });
                }
            }

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

            // xử lý khi chỉ map các property có thay đổi của Data DTO
            if (sourceType.IsAssignableTo<IDto>())
            {
                foreach (var property in entityType.GetProperties())
                {
                    map.ForMember(property.Name, opt =>
                    {
                        opt.Condition((
                            source,
                            desc,
                            sourceVal,
                            descVal,
                            context) =>
                        {
                            if (!context.Items.ContainsKey("mapPropertyChangedOnly")) return true;
                            var mode = (string)context.Items["mapPropertyChangedOnly"];
                            if (mode == null) return true;

                            var sourceData = source as IDto;
                            return mode.Trim() == "true" && sourceData != null
                                && sourceData.IsPropertyChanged(property.Name);
                        });
                    });
                }
            }

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
