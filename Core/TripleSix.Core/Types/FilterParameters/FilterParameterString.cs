using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using TripleSix.Core.Entities;
using TripleSix.Core.Exceptions;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Types
{
    public enum FilterParameterStringOperators
    {
        [Description("==")]
        Equal = 1,

        [Description("contain")]
        Contain = 2,

        [Description("start")]
        StartWith = 3,

        [Description("end")]
        EndWith = 4,

        [Description("in")]
        In = 5,

        [Description("null")]
        IsNull = 6,

        [Description("!=")]
        NotEqual = -1,

        [Description("!contain")]
        NotContain = -2,

        [Description("!start")]
        NotStartWith = -3,

        [Description("!end")]
        NotEndWith = -4,

        [Description("!in")]
        NotIn = -5,

        [Description("!null")]
        NotNull = -6,
    }

    public class FilterParameterString : IFilterParameter
    {
        [DisplayName("[parameter-display-name]")]
        public string?[]? Value { get; set; }

        [DisplayName("loại lọc của [parameter-name]")]
        public FilterParameterStringOperators Operator { get; set; } = FilterParameterStringOperators.Equal;

        public IQueryable<TEntity> ToQueryable<TEntity>(IQueryable<TEntity> query, string propertyName)
            where TEntity : class, IEntity
        {
            switch (Operator)
            {
                case FilterParameterStringOperators.Equal:
                    if (Value.IsNullOrEmpty() || Value[0] == null)
                        throw new InputInvalidException(propertyName, "Thiếu dữ liệu để lọc");
                    query = query.Where(x => EF.Property<string>(x, propertyName) == Value[0]);
                    break;

                case FilterParameterStringOperators.Contain:
                    if (Value.IsNullOrEmpty() || Value[0] == null)
                        throw new InputInvalidException(propertyName, "Thiếu dữ liệu để lọc");
                    query = query.Where(x => EF.Functions.Like(EF.Property<string>(x, propertyName), $"%{Value[0]}%"));
                    break;

                case FilterParameterStringOperators.StartWith:
                    if (Value.IsNullOrEmpty() || Value[0] == null)
                        throw new InputInvalidException(propertyName, "Thiếu dữ liệu để lọc");
                    query = query.Where(x => EF.Functions.Like(EF.Property<string>(x, propertyName), $"{Value[0]}%"));
                    break;

                case FilterParameterStringOperators.EndWith:
                    if (Value.IsNullOrEmpty() || Value[0] == null)
                        throw new InputInvalidException(propertyName, "Thiếu dữ liệu để lọc");
                    query = query.Where(x => EF.Functions.Like(EF.Property<string>(x, propertyName), $"%{Value[0]}"));
                    break;

                case FilterParameterStringOperators.In:
                    if (Value.IsNullOrEmpty())
                        throw new InputInvalidException(propertyName, "Thiếu dữ liệu để lọc");
                    query = query.Where(x => Value.Contains(EF.Property<string>(x, propertyName)));
                    break;

                case FilterParameterStringOperators.IsNull:
                    query = query.Where(x => EF.Property<string>(x, propertyName) == null);
                    break;

                case FilterParameterStringOperators.NotEqual:
                    if (Value.IsNullOrEmpty() || Value[0] == null)
                        throw new InputInvalidException(propertyName, "Thiếu dữ liệu để lọc");
                    query = query.Where(x => EF.Property<string>(x, propertyName) != Value[0]);
                    break;

                case FilterParameterStringOperators.NotContain:
                    if (Value.IsNullOrEmpty() || Value[0] == null)
                        throw new InputInvalidException(propertyName, "Thiếu dữ liệu để lọc");
                    query = query.Where(x => !EF.Functions.Like(EF.Property<string>(x, propertyName), $"%{Value[0]}%"));
                    break;

                case FilterParameterStringOperators.NotStartWith:
                    if (Value.IsNullOrEmpty() || Value[0] == null)
                        throw new InputInvalidException(propertyName, "Thiếu dữ liệu để lọc");
                    query = query.Where(x => !EF.Functions.Like(EF.Property<string>(x, propertyName), $"%{Value[0]}"));
                    break;

                case FilterParameterStringOperators.NotEndWith:
                    if (Value.IsNullOrEmpty() || Value[0] == null)
                        throw new InputInvalidException(propertyName, "Thiếu dữ liệu để lọc");
                    query = query.Where(x => !EF.Functions.Like(EF.Property<string>(x, propertyName), $"{Value[0]}%"));
                    break;

                case FilterParameterStringOperators.NotIn:
                    if (Value.IsNullOrEmpty())
                        throw new InputInvalidException(propertyName, "Thiếu dữ liệu để lọc");
                    query = query.Where(x => !Value.Contains(EF.Property<string>(x, propertyName)));
                    break;

                case FilterParameterStringOperators.NotNull:
                    query = query.Where(x => EF.Property<string>(x, propertyName) != null);
                    break;
            }

            return query;
        }
    }
}
