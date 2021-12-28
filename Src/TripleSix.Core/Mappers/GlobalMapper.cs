using System;
using System.Collections.Generic;
using AutoMapper;
using TripleSix.Core.DataTypes;
using TripleSix.Core.Dto;
using TripleSix.Core.Entities;
using TripleSix.Core.Extensions;

namespace TripleSix.Core.Mappers
{
    public abstract class GlobalMapper : BaseMapper
    {
        protected GlobalMapper()
        {
            CreateProfile();
            CreateMap<string, Phone>().ConvertUsing(s => new Phone(s));
            CreateMap<Phone, string>().ConvertUsing(s => s == null ? null : s.ToString());
        }

        protected abstract IEnumerable<Type> SelectEntity();

        protected abstract IEnumerable<Type> SelectDto(string objectName);

        private void CreateProfile()
        {
            var entities = SelectEntity();
            foreach (var entity in entities)
            {
                var objectName = entity.Name.Substring(0, entity.Name.IndexOf("Entity", StringComparison.Ordinal));

                CreateMap(entity, entity).IncludeAllDerived();
                RegisterMap(entity, typeof(ModelDataDto));

                var dtos = SelectDto(objectName);
                foreach (var dto in dtos)
                {
                    CreateMap(typeof(ModelDataDto), dto, MemberList.None).IncludeAllDerived();
                    RegisterMap(entity, dto);
                }
            }
        }

        private void RegisterMap(Type entity, Type dto)
        {
            var map = CreateMap(entity, dto, MemberList.None)
                .IncludeAllDerived()
                .ReverseMap();

            foreach (var p in entity.GetProperties())
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
    }
}