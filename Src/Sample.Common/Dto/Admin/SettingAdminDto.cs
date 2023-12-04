using System.ComponentModel;
using TripleSix.CoreOld.AutoAdmin;
using TripleSix.CoreOld.Dto;

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

        public class Update : DataDto
        {
            [DisplayName("giá trị")]
            public string Value { get; set; }

            [DisplayName("mô tả")]
            public string Description { get; set; }
        }
    }
}
