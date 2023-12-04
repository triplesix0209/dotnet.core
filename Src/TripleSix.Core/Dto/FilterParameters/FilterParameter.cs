#pragma warning disable SA1201 // Elements should appear in the correct order

using System.ComponentModel;
using TripleSix.Core.Attributes;

namespace TripleSix.Core.Dto
{
    public class FilterParameter<TType> : IFilterParameter
    {
        [DisplayName("[parameter-display-name]")]
        public TType[] Value { get; set; }

        [DisplayName("loại lọc của [parameter-name]")]
        [EnumValidate]
        public FilterParameterOperators Operator { get; set; } = FilterParameterOperators.Equal;
    }

    public enum FilterParameterOperators
    {
        [Description("chính xác")]
        Equal = 1,

        [Description("thuộc danh sách")]
        In = 2,

        [Description("là NULL")]
        IsNull = 3,

        [Description("khác")]
        NotEqual = -1,

        [Description("không thuộc danh sách")]
        NotIn = -2,

        [Description("không NULL")]
        NotNull = -3,
    }
}
