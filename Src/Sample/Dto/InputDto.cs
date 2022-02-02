using TripleSix.Core.Attributes;
using TripleSix.Core.Dto;

namespace Sample.Dto
{
    public class InputDto : DataDto
    {
        public int Id { get; set; }

        [RequiredValidate]
        public string Name { get; set; }
    }
}
