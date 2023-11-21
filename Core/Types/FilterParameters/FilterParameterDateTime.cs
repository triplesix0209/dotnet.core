using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using TripleSix.Core.Entities;
using TripleSix.Core.Exceptions;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Types
{
    public enum FilterParameterDatetimeOperators
    {
        [Description("==")]
        Equal = 1,

        [Description(">=")]
        Begin = 2,

        [Description("<=")]
        End = 3,

        [Description("range")]
        Between = 4,

        [Description("null")]
        IsNull = 5,

        [Description("!=")]
        NotEqual = -1,

        [Description("!range")]
        NotBetween = -4,

        [Description("!null")]
        NotNull = -5,
    }

    public class FilterParameterDatetime : IFilterParameter
    {
        [DisplayName("[parameter-display-name]")]
        public DateTime?[]? Value { get; set; }

        [DisplayName("loại lọc của [parameter-name]")]
        public FilterParameterDatetimeOperators Operator { get; set; } = FilterParameterDatetimeOperators.Equal;

        /// <inheritdoc/>
        public IQueryable<TEntity> ToQueryable<TEntity>(IQueryable<TEntity> query, string propertyName)
            where TEntity : class, IEntity
        {
            switch (Operator)
            {
                case FilterParameterDatetimeOperators.Equal:
                    if (Value.IsNullOrEmpty() || Value[0] == null)
                        throw new InputInvalidException(propertyName, "Thiếu dữ liệu để lọc");
                    query = query.Where(x => EF.Property<DateTime>(x, propertyName) == Value[0]);
                    break;

                case FilterParameterDatetimeOperators.Begin:
                    if (Value.IsNullOrEmpty() || Value[0] == null)
                        throw new InputInvalidException(propertyName, "Thiếu dữ liệu để lọc");
                    query = query.Where(x => EF.Property<DateTime>(x, propertyName) >= Value[0]);
                    break;

                case FilterParameterDatetimeOperators.End:
                    if (Value.IsNullOrEmpty() || Value[0] == null)
                        throw new InputInvalidException(propertyName, "Thiếu dữ liệu để lọc");
                    query = query.Where(x => EF.Property<DateTime>(x, propertyName) <= Value[0]);
                    break;

                case FilterParameterDatetimeOperators.Between:
                    if (Value.IsNullOrEmpty() || Value.Length < 2 || Value[0] == null || Value[1] == null)
                        throw new InputInvalidException(propertyName, "Thiếu dữ liệu để lọc");
                    query = query.Where(x => EF.Property<DateTime>(x, propertyName) >= Value[0]
                        && EF.Property<DateTime>(x, propertyName) <= Value[1]);
                    break;

                case FilterParameterDatetimeOperators.IsNull:
                    query = query.Where(x => EF.Property<DateTime?>(x, propertyName) == null);
                    break;

                case FilterParameterDatetimeOperators.NotEqual:
                    if (Value.IsNullOrEmpty() || Value[0] == null)
                        throw new InputInvalidException(propertyName, "Thiếu dữ liệu để lọc");
                    query = query.Where(x => EF.Property<DateTime>(x, propertyName) != Value[0]);
                    break;

                case FilterParameterDatetimeOperators.NotBetween:
                    if (Value.IsNullOrEmpty() || Value.Length < 2 || Value[0] == null || Value[1] == null)
                        throw new InputInvalidException(propertyName, "Thiếu dữ liệu để lọc");
                    query = query.Where(x => EF.Property<DateTime>(x, propertyName) < Value[0]
                        || EF.Property<DateTime>(x, propertyName) > Value[1]);
                    break;

                case FilterParameterDatetimeOperators.NotNull:
                    query = query.Where(x => EF.Property<DateTime?>(x, propertyName) != null);
                    break;
            }

            return query;
        }
    }
}
