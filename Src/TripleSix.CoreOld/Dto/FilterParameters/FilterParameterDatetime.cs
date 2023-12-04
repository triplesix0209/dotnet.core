#pragma warning disable SA1201 // Elements should appear in the correct order

using System.ComponentModel;
using TripleSix.CoreOld.Attributes;

namespace TripleSix.CoreOld.Dto
{
    public class FilterParameterDatetime : IFilterParameter
    {
        [DisplayName("[parameter-display-name]")]
        public long[] Value { get; set; }

        [DisplayName("loại lọc của [parameter-name]")]
        [EnumValidate]
        public FilterParameterDatetimeOperators Operator { get; set; } = FilterParameterDatetimeOperators.Equal;
    }

    public enum FilterParameterDatetimeOperators
    {
        [Description("chính xác")]
        Equal = 1,

        [Description("bắt đầu lúc")]
        Begin = 2,

        [Description("kết thúc lúc")]
        End = 3,

        [Description("trong khoảng")]
        Between = 4,

        [Description("là NULL")]
        IsNull = 5,

        [Description("khác")]
        NotEqual = -1,

        [Description("không bắt đầu lúc")]
        NotBegin = -2,

        [Description("không kết thúc lúc")]
        NotEnd = -3,

        [Description("không trong khoảng")]
        NotBetween = -4,

        [Description("không NULL")]
        NotNull = -5,
    }
}
