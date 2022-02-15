using System.ComponentModel;
using TripleSix.Core.Attributes;
using TripleSix.Core.Enums;

namespace TripleSix.Core.Dto
{
    public class FilterParameter<TType> : IFilterParameter
    {
        [DisplayName("[parameter-display-name]")]
        public TType Value { get; set; }

        [DisplayName("loại lọc của [parameter-name]")]
        [EnumValidate]
        public FilterParameterTypes FilterType { get; set; }
    }
}
