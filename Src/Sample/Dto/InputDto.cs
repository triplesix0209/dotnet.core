using TripleSix.Core.Attributes;
using TripleSix.Core.Dto;

namespace Sample.Dto
{
    public class InputDto : DataDto
    {
        [RequiredValidate]
        public string Data { get; set; }
    }
}
