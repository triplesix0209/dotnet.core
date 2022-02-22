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
        [Description("Is")]
        Is = 1,

        [Description("Begin")]
        Begin = 2,

        [Description("End")]
        End = 3,

        [Description("Between")]
        Between = 4,

        [Description("Is NULL")]
        IsNull = 5,

        [Description("Not")]
        NotIs = -1,

        [Description("Not begin")]
        NotBegin = -2,

        [Description("Not end")]
        NotEnd = -3,

        [Description("Not between")]
        NotBetween = -4,

        [Description("Not NULL")]
        NotNull = -5,
    }
}
