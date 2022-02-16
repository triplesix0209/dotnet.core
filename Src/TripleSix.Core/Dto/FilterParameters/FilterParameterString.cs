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
        Equal = 1,
        Contain = 2,
        StartWith = 3,
        EndWith = 4,
        In = 5,
        IsNull = 6,
        NotEqual = -1,
        NotContain = -2,
        NotStartWith = -3,
        NotEndWith = -4,
        NotIn = -5,
        NotNull = -6,
    }
}
