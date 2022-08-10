using System.ComponentModel;
using TripleSix.Core.Entities;

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
    {
        [DisplayName("[parameter-display-name]")]
        public TType[] Value { get; set; }

        [DisplayName("loại lọc của [parameter-name]")]
        public FilterParameterOperators Operator { get; set; } = FilterParameterOperators.Equal;

        public IQueryable<TEntity> ToQueryable<TEntity>(IQueryable<TEntity> query)
            where TEntity : class, IEntity
        {
            return query;
        }
    }
}
