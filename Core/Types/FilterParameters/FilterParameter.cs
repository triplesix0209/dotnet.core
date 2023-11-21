using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using TripleSix.Core.Entities;
using TripleSix.Core.Exceptions;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Types
{
    public enum FilterParameterOperators
    {
        [Description("==")]
        Equal = 1,

        [Description("in")]
        In = 2,

        [Description("null")]
        IsNull = 3,

        [Description("!=")]
        NotEqual = -1,

        [Description("!in")]
        NotIn = -2,

        [Description("!null")]
        NotNull = -3,
    }

    public class FilterParameter<TType> : IFilterParameter
        where TType : notnull
    {
        [DisplayName("[parameter-display-name]")]
        public TType?[]? Value { get; set; }

        [DisplayName("loại lọc của [parameter-name]")]
        public FilterParameterOperators Operator { get; set; } = FilterParameterOperators.Equal;

        /// <inheritdoc/>
        public IQueryable<TEntity> ToQueryable<TEntity>(IQueryable<TEntity> query, string propertyName)
            where TEntity : class, IEntity
        {
            switch (Operator)
            {
                case FilterParameterOperators.Equal:
                    if (Value.IsNullOrEmpty() || Value[0] == null)
                        throw new InputInvalidException(propertyName, "Thiếu dữ liệu để lọc");
                    query = query.Where(x => EF.Property<TType>(x, propertyName).Equals(Value[0]));
                    break;

                case FilterParameterOperators.In:
                    if (Value.IsNullOrEmpty())
                        throw new InputInvalidException(propertyName, "Thiếu dữ liệu để lọc");
                    query = query.Where(x => Value.Contains(EF.Property<TType>(x, propertyName)));
                    break;

                case FilterParameterOperators.IsNull:
                    query = query.Where(x => EF.Property<TType>(x, propertyName) == null);
                    break;

                case FilterParameterOperators.NotEqual:
                    if (Value.IsNullOrEmpty() || Value[0] == null)
                        throw new InputInvalidException(propertyName, "Thiếu dữ liệu để lọc");
                    query = query.Where(x => !EF.Property<TType>(x, propertyName).Equals(Value[0]));
                    break;

                case FilterParameterOperators.NotIn:
                    if (Value.IsNullOrEmpty())
                        throw new InputInvalidException(propertyName, "Thiếu dữ liệu để lọc");
                    query = query.Where(x => !Value.Contains(EF.Property<TType>(x, propertyName)));
                    break;

                case FilterParameterOperators.NotNull:
                    query = query.Where(x => EF.Property<TType>(x, propertyName) != null);
                    break;
            }

            return query;
        }
    }
}
