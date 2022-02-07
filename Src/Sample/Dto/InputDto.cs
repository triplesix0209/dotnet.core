using System.ComponentModel;
using Sample.Enums;
using TripleSix.Core.Attributes;
using TripleSix.Core.Dto;

namespace Sample.Dto
{
    public class InputDto : DataDto
    {
        [DisplayName("Mã định danh")]
        [RequiredValidate]
        public int Id { get; set; }

        [DisplayName("Năm sản xuất")]
        [RangeValidate(1900, 2100)]
        public int Year { get; set; } = 2022;

        [DisplayName("Số hiệu")]
        [MinValidate(0)]
        [MaxValidate(999)]
        public int Number { get; set; }

        [DisplayName("Tên gọi")]
        [StringLengthValidate(100)]
        public string Name { get; set; }

        [DisplayName("Phân loại")]
        [EnumValidate]
        public MonsterRaces Race { get; set; }

        [DisplayName("Data")]
        public bool Data { get; set; }
    }
}
