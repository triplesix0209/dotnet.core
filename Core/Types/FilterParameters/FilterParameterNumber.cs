using System.ComponentModel;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TripleSix.Core.Entities;
using TripleSix.Core.Exceptions;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Types
{
    public enum FilterParameterNumberOperators
    {
        [Description("==")]
        Equal = 1,

        [Description("<")]
        Less = 2,

        [Description("<=")]
        LessOrEqual = 3,

        [Description(">")]
        Greater = 4,

        [Description(">=")]
        GreaterOrEqual = 5,

        [Description("in")]
        In = 6,

        [Description("null")]
        IsNull = 7,

        [Description("!=")]
        NotEqual = -1,

        [Description("!in")]
        NotIn = -6,

        [Description("!null")]
        NotNull = -7,
    }

    public class FilterParameterNumber<TType> : IFilterParameter
        where TType : IComparable
    {
        [DisplayName("[parameter-display-name]")]
        public TType?[]? Value { get; set; }

        [DisplayName("loại lọc của [parameter-name]")]
        public FilterParameterNumberOperators Operator { get; set; } = FilterParameterNumberOperators.Equal;

        public IQueryable<TEntity> ToQueryable<TEntity>(IQueryable<TEntity> query, string propertyName)
            where TEntity : class, IEntity
        {
            var pe = Expression.Parameter(typeof(TEntity));
            Expression expr;

            switch (Operator)
            {
                case FilterParameterNumberOperators.Equal:
                    if (Value.IsNullOrEmpty() || Value[0] == null)
                        throw new InputInvalidException(propertyName, "Thiếu dữ liệu để lọc");
                    query = query.Where(x => EF.Property<TType>(x, propertyName).Equals(Value[0]));
                    break;

                case FilterParameterNumberOperators.Less:
                    if (Value.IsNullOrEmpty() || Value[0] == null)
                        throw new InputInvalidException(propertyName, "Thiếu dữ liệu để lọc");
                    expr = Expression.LessThan(
                        Expression.Convert(Expression.Property(pe, propertyName), typeof(int)),
                        Expression.Constant(Value[0]));
                    query = query.Where(Expression.Lambda<Func<TEntity, bool>>(expr, pe));
                    break;

                case FilterParameterNumberOperators.LessOrEqual:
                    if (Value.IsNullOrEmpty() || Value[0] == null)
                        throw new InputInvalidException(propertyName, "Thiếu dữ liệu để lọc");
                    expr = Expression.LessThanOrEqual(
                        Expression.Convert(Expression.Property(pe, propertyName), typeof(int)),
                        Expression.Constant(Value[0]));
                    query = query.Where(Expression.Lambda<Func<TEntity, bool>>(expr, pe));
                    break;

                case FilterParameterNumberOperators.Greater:
                    if (Value.IsNullOrEmpty() || Value[0] == null)
                        throw new InputInvalidException(propertyName, "Thiếu dữ liệu để lọc");
                    expr = Expression.GreaterThan(
                        Expression.Convert(Expression.Property(pe, propertyName), typeof(int)),
                        Expression.Constant(Value[0]));
                    query = query.Where(Expression.Lambda<Func<TEntity, bool>>(expr, pe));
                    break;

                case FilterParameterNumberOperators.GreaterOrEqual:
                    if (Value.IsNullOrEmpty() || Value[0] == null)
                        throw new InputInvalidException(propertyName, "Thiếu dữ liệu để lọc");
                    expr = Expression.GreaterThanOrEqual(
                        Expression.Convert(Expression.Property(pe, propertyName), typeof(int)),
                        Expression.Constant(Value[0]));
                    query = query.Where(Expression.Lambda<Func<TEntity, bool>>(expr, pe));
                    break;

                case FilterParameterNumberOperators.In:
                    if (Value.IsNullOrEmpty())
                        throw new InputInvalidException(propertyName, "Thiếu dữ liệu để lọc");
                    query = query.Where(x => Value.Contains(EF.Property<TType>(x, propertyName)));
                    break;

                case FilterParameterNumberOperators.IsNull:
                    query = query.Where(x => EF.Property<TType>(x, propertyName) == null);
                    break;

                case FilterParameterNumberOperators.NotEqual:
                    if (Value.IsNullOrEmpty() || Value[0] == null)
                        throw new InputInvalidException(propertyName, "Thiếu dữ liệu để lọc");
                    query = query.Where(x => !EF.Property<TType>(x, propertyName).Equals(Value[0]));
                    break;

                case FilterParameterNumberOperators.NotIn:
                    if (Value.IsNullOrEmpty())
                        throw new InputInvalidException(propertyName, "Thiếu dữ liệu để lọc");
                    query = query.Where(x => !Value.Contains(EF.Property<TType>(x, propertyName)));
                    break;

                case FilterParameterNumberOperators.NotNull:
                    query = query.Where(x => EF.Property<TType>(x, propertyName) != null);
                    break;
            }

            return query;
        }
    }
}
