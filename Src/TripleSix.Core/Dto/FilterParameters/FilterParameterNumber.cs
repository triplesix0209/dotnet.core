#pragma warning disable SA1201 // Elements should appear in the correct order

using System;
using System.ComponentModel;
using TripleSix.Core.Attributes;

namespace TripleSix.Core.Dto
{
    public class FilterParameterNumber<TType> : IFilterParameter
        where TType : IComparable
    {
        [DisplayName("[parameter-display-name]")]
        public TType[] Value { get; set; }

        [DisplayName("loại lọc của [parameter-name]")]
        [EnumValidate]
        public FilterParameterNumberOperators Operator { get; set; } = FilterParameterNumberOperators.Equal;
    }

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

        [Description("In")]
        In = 6,

        [Description("Is NULL")]
        IsNull = 7,

        [Description("!=")]
        NotEqual = -1,

        [Description("Not in")]
        NotIn = -6,

        [Description("Not NULL")]
        NotNull = -7,
    }
}
