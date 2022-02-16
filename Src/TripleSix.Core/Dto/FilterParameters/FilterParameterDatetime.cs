#pragma warning disable SA1201 // Elements should appear in the correct order

using System;
using System.ComponentModel;
using TripleSix.Core.Attributes;

namespace TripleSix.Core.Dto
{
    public class FilterParameterDatetime : IFilterParameter
    {
        [DisplayName("[parameter-display-name]")]
        public DateTime[] Value { get; set; }

        [DisplayName("loại lọc của [parameter-name]")]
        [EnumValidate]
        public FilterParameterDatetimeOperators Operator { get; set; } = FilterParameterDatetimeOperators.Is;
    }

    public enum FilterParameterDatetimeOperators
    {
        Is = 1,
        Begin = 2,
        End = 3,
        Between = 4,
        IsNull = 5,
        NotIs = -1,
        NotBegin = -2,
        NotEnd = -3,
        NotBetween = -4,
        NotNull = -5,
    }
}
