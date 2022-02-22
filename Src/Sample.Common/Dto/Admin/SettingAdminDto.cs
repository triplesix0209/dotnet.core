using System.ComponentModel;
using TripleSix.Core.Attributes;
using TripleSix.Core.AutoAdmin;
using TripleSix.Core.Dto;

namespace Sample.Common.Dto
{
    public class SettingAdminDto : BaseAdminDto
    {
        public class Filter : ModelFilterDto
        {
            [DisplayName("lọc theo mô tả")]
            public FilterParameterString Description { get; set; }
        }

        public class Item : ModelDataDto
        {
            [DisplayName("giá trị")]
            public string Value { get; set; }

            [DisplayName("mô tả")]
            public string Description { get; set; }
        }

        public class Detail : Item
        {
        }

        public class Create : DataDto
        {
            [DisplayName("mã số")]
            [RequiredValidate]
            [StringLengthValidate(100)]
            public string Code { get; set; }

            [DisplayName("giá trị")]
            public string Value { get; set; }

            [DisplayName("mô tả")]
            public string Description { get; set; }
        }

        public class Update : DataDto
        {
            [DisplayName("giá trị")]
            public string Value { get; set; }

            [DisplayName("mô tả")]
            public string Description { get; set; }
        }
    }
}
