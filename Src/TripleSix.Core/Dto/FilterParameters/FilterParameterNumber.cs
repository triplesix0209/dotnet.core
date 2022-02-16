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
        Equal = 1,
        Less = 2,
        LessOrEqual = 3,
        Greater = 4,
        GreaterOrEqual = 5,
        In = 6,
        IsNull = 7,
        NotEqual = -1,
        NotIn = -6,
        NotNull = -7,
    }
}
