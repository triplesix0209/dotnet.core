using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TripleSix.Core.Dto;
using TripleSix.Core.Entities;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Mappers
{
    public abstract class BaseMapper : Profile
    {
        protected new IMappingExpression CreateMap(
            Type sourceType,
            Type destinationType,
            MemberList memberList = MemberList.Destination)
        {
            var map = base.CreateMap(sourceType, destinationType, memberList);
            if (sourceType.IsAssignableTo<IPropertyTracking>())
            {
                foreach (var property in destinationType.GetProperties())
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
                            if (!context.Items.ContainsKey("mode")) return true;
                            var mode = (string)context.Items["mode"];
                            if (mode == null) return true;

                            return mode == "update" && ((IPropertyTracking)source).IsPropertyChanged(property.Name);
                        });
                    });
                }
            }

            return map;
        }

        protected new IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>(
            MemberList memberList = MemberList.Destination)
        {
            var map = base.CreateMap<TSource, TDestination>(memberList);
            if (typeof(TSource).IsAssignableTo<IPropertyTracking>())
            {
                foreach (var property in typeof(TDestination).GetProperties())
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
                            if (!context.Items.ContainsKey("mode")) return true;
                            var mode = (string)context.Items["mode"];
                            if (mode == null) return true;

                            return mode == "update" && ((IPropertyTracking)source).IsPropertyChanged(property.Name);
                        });
                    });
                }
            }

            return map;
        }

        protected IMappingExpression CreateMapToEntity(
            Type sourceType,
            Type destinationType,
            MemberList memberList = MemberList.Destination)
        {
            if (!destinationType.IsAssignableTo<IEntity>())
                throw new Exception($"{destinationType.Name} need implement {nameof(IEntity)}> interface");

            var map = CreateMap(sourceType, destinationType, memberList);
            var ignoreProperties = destinationType.GetProperties()
                .Where(property =>
                {
                    var propertyType = property.PropertyType.GetUnderlyingType();
                    return propertyType.IsAssignableTo<IEntity>()
                        || propertyType.IsSubclassOfRawGeneric(typeof(ICollection<>))
                        || propertyType.IsSubclassOfRawGeneric(typeof(IList<>));
                });
            foreach (var property in ignoreProperties)
                map.ForMember(property.Name, opt => opt.Ignore());

            return map;
        }

        protected IMappingExpression<TSource, TEntity> CreateMapToEntity<TSource, TEntity>(
            MemberList memberList = MemberList.Destination)
            where TEntity : IEntity
        {
            var map = CreateMap<TSource, TEntity>(memberList);
            var ignoreProperties = typeof(TEntity).GetProperties()
                .Where(property =>
                {
                    var propertyType = property.PropertyType.GetUnderlyingType();
                    return propertyType.IsAssignableTo<IEntity>()
                        || propertyType.IsSubclassOfRawGeneric(typeof(ICollection<>))
                        || propertyType.IsSubclassOfRawGeneric(typeof(IList<>));
                });
            foreach (var property in ignoreProperties)
                map.ForMember(property.Name, opt => opt.Ignore());

            return map;
        }

        protected IMappingExpression CreateMapFromEntity(
            Type sourceType,
            Type destinationType,
            MemberList memberList = MemberList.Destination)
        {
            if (!sourceType.IsAssignableTo<IEntity>())
                throw new Exception($"{sourceType.Name} need implement {nameof(IEntity)}> interface");

            var map = CreateMap(sourceType, destinationType, memberList);
            var ignoreProperties = sourceType.GetProperties()
                .Where(property =>
                {
                    var propertyType = property.PropertyType.GetUnderlyingType();
                    return propertyType.IsAssignableTo<IEntity>()
                        || propertyType.IsSubclassOfRawGeneric(typeof(ICollection<>))
                        || propertyType.IsSubclassOfRawGeneric(typeof(IList<>));
                });
            foreach (var property in ignoreProperties)
                map.ForSourceMember(property.Name, opt => opt.DoNotValidate());

            return map;
        }

        protected IMappingExpression<TEntity, TDestination> CreateMapFromEntity<TEntity, TDestination>(
            MemberList memberList = MemberList.Destination)
            where TEntity : IEntity
        {
            var map = CreateMap<TEntity, TDestination>(memberList);
            var ignoreProperties = typeof(TEntity).GetProperties()
                .Where(property =>
                {
                    var propertyType = property.PropertyType.GetUnderlyingType();
                    return propertyType.IsAssignableTo<IEntity>()
                        || propertyType.IsSubclassOfRawGeneric(typeof(ICollection<>))
                        || propertyType.IsSubclassOfRawGeneric(typeof(IList<>));
                });
            foreach (var property in ignoreProperties)
                map.ForSourceMember(property.Name, opt => opt.DoNotValidate());

            return map;
        }
    }
}
