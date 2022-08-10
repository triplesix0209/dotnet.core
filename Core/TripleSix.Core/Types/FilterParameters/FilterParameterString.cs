using System.ComponentModel;

namespace TripleSix.Core.Types
{
    public enum FilterParameterStringOperators
    {
        [Description("==")]
        Equal = 1,

        [Description("contain")]
        Contain = 2,

        [Description("start")]
        StartWith = 3,

        [Description("end")]
        EndWith = 4,

        [Description("in")]
        In = 5,

        [Description("null")]
        IsNull = 6,

        [Description("!=")]
        NotEqual = -1,

        [Description("!contain")]
        NotContain = -2,

        [Description("!start")]
        NotStartWith = -3,

        [Description("!end")]
        NotEndWith = -4,

        [Description("!in")]
        NotIn = -5,

        [Description("!null")]
        NotNull = -6,
    }

    public class FilterParameterString : IFilterParameter
    {
        [DisplayName("[parameter-display-name]")]
        public string[] Value { get; set; }

        [DisplayName("loại lọc của [parameter-name]")]
        public FilterParameterStringOperators Operator { get; set; } = FilterParameterStringOperators.Equal;
    }
}
