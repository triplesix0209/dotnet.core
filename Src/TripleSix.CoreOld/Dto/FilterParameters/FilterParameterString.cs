#pragma warning disable SA1201 // Elements should appear in the correct order

using System.ComponentModel;
using TripleSix.CoreOld.Attributes;

namespace TripleSix.CoreOld.Dto
{
    public class FilterParameterString : IFilterParameter
    {
        [DisplayName("[parameter-display-name]")]
        public string[] Value { get; set; }

        [DisplayName("loại lọc của [parameter-name]")]
        [EnumValidate]
        public FilterParameterStringOperators Operator { get; set; } = FilterParameterStringOperators.Equal;
    }

    public enum FilterParameterStringOperators
    {
        [Description("chính xác")]
        Equal = 1,

        [Description("chứa")]
        Contain = 2,

        [Description("bắt đầu với")]
        StartWith = 3,

        [Description("kết thúc với")]
        EndWith = 4,

        [Description("thuộc danh sách")]
        In = 5,

        [Description("là NULL")]
        IsNull = 6,

        [Description("khác")]
        NotEqual = -1,

        [Description("không chứa")]
        NotContain = -2,

        [Description("không bắt đầu với")]
        NotStartWith = -3,

        [Description("không kết thúc với")]
        NotEndWith = -4,

        [Description("không thuộc danh sách")]
        NotIn = -5,

        [Description("không NULL")]
        NotNull = -6,
    }
}
