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
        [Description("Is")]
        Is = 1,

        [Description("In")]
        In = 2,

        [Description("Is NULL")]
        IsNull = 3,

        [Description("Not")]
        NotIs = -1,

        [Description("Not in")]
        NotIn = -2,

        [Description("Not NULL")]
        NotNull = -3,
    }
}
