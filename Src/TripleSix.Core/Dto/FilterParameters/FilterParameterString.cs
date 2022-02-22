#pragma warning disable SA1201 // Elements should appear in the correct order

using System.ComponentModel;
using TripleSix.Core.Attributes;

namespace TripleSix.Core.Dto
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
        [Description("Equal")]
        Equal = 1,

        [Description("Contain")]
        Contain = 2,

        [Description("Start with")]
        StartWith = 3,

        [Description("End with")]
        EndWith = 4,

        [Description("In")]
        In = 5,

        [Description("Is NULL")]
        IsNull = 6,

        [Description("Not equal")]
        NotEqual = -1,

        [Description("Not contain")]
        NotContain = -2,

        [Description("Not start with")]
        NotStartWith = -3,

        [Description("Not end with")]
        NotEndWith = -4,

        [Description("Not in")]
        NotIn = -5,

        [Description("Not NULL")]
        NotNull = -6,
    }
}
