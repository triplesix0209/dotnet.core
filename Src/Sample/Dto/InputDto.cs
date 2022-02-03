using Sample.Enums;
using TripleSix.Core.Attributes;
using TripleSix.Core.Dto;

namespace Sample.Dto
{
    public class InputDto : DataDto
    {
        [MinValidate(1)]
        public int Id { get; set; }

        [RequiredValidate]
        public string Name { get; set; }

        [EnumValidate]
        public MonsterRaces Race { get; set; }
    }
}
