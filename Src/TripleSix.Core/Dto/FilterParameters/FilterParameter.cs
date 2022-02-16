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
        public FilterParameterOperators Operator { get; set; } = FilterParameterOperators.Is;
    }

    public enum FilterParameterOperators
    {
        Is = 1,
        In = 2,
        IsNull = 3,
        NotIs = -1,
        NotIn = -2,
        NotNull = -3,
    }
}
