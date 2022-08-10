using System.ComponentModel;

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

        [Description("!>=")]
        NotBegin = -2,

        [Description("!<=")]
        NotEnd = -3,

        [Description("!range")]
        NotBetween = -4,

        [Description("!null")]
        NotNull = -5,
    }

    public class FilterParameterDatetime : IFilterParameter
    {
        [DisplayName("[parameter-display-name]")]
        public long[] Value { get; set; }

        [DisplayName("loại lọc của [parameter-name]")]
        public FilterParameterDatetimeOperators Operator { get; set; } = FilterParameterDatetimeOperators.Equal;
    }
}
