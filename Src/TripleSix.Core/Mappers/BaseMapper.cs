using System;
using System.Collections.Generic;
using AutoMapper;
using TripleSix.Core.Dto;
using TripleSix.Core.Entities;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Mappers
{
    public abstract class BaseMapper : Profile
    {
        protected new IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>(
            MemberList memberList = MemberList.Destination)
        {
            var map = base.CreateMap<TSource, TDestination>(memberList)
                .IncludeAllDerived();

            var destType = typeof(TDestination);
            if (typeof(IPropertyTracking).IsAssignableFrom(typeof(TSource)))
            {
                foreach (var p in destType.GetProperties())
                {
                    map.ForMember(p.Name, opt =>
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

                            return mode == "update" && ((IPropertyTracking)source).IsPropertyChanged(p.Name);
                        });
                    });
                }
            }

            return map;
        }

        protected IMappingExpression<TSource, TEntity> CreateMapToEntity<TSource, TEntity>(
            MemberList memberList = MemberList.Destination)
            where TEntity : IEntity
        {
            var map = base.CreateMap<TSource, TEntity>(memberList)
                .IncludeAllDerived();

            var isPropertyTracking = typeof(IPropertyTracking).IsAssignableFrom(typeof(TSource));
            var entityType = typeof(TEntity);
            foreach (var p in entityType.GetProperties())
            {
                var descType = Nullable.GetUnderlyingType(p.PropertyType) != null
                    ? Nullable.GetUnderlyingType(p.PropertyType)
                    : p.PropertyType;

                if (typeof(IEntity).IsAssignableFrom(descType)
                    || descType.IsSubclassOfRawGeneric(typeof(ICollection<>))
                    || descType.IsSubclassOfRawGeneric(typeof(IList<>)))
                {
                    map.ForMember(p.Name, opt => opt.Ignore());
                    continue;
                }

                if (isPropertyTracking)
                {
                    map.ForMember(p.Name, opt =>
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

                            return mode == "update" && ((IPropertyTracking)source).IsPropertyChanged(p.Name);
                        });
                    });
                }
            }

            return map;
        }
    }
}