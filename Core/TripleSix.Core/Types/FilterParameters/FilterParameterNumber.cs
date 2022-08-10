using System.ComponentModel;
using TripleSix.Core.Entities;

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
        public TType[] Value { get; set; }

        [DisplayName("loại lọc của [parameter-name]")]
        public FilterParameterNumberOperators Operator { get; set; } = FilterParameterNumberOperators.Equal;

        public IQueryable<TEntity> ToQueryable<TEntity>(IQueryable<TEntity> query)
            where TEntity : class, IEntity
        {
            return query;
        }
    }
}
