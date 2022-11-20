using System.ComponentModel;
using TripleSix.Core.Attributes;
using TripleSix.Core.AutoAdmin;
using TripleSix.Core.Dto;

namespace Sample.Common.Dto
{
    public class TestAdminDto : BaseAdminDto
    {
        public class Filter : ModelFilterDto
        {
            [DisplayName("lọc theo tên")]
            public FilterParameterString Name { get; set; }
        }

        public class Item : ModelDataDto
        {
            [DisplayName("tên")]
            public string Name { get; set; }
        }

        public class Detail : Item
        {
        }

        public class Create : DataDto
        {
            [DisplayName("tên gọi")]
            [StringLengthValidate(100)]
            [RequiredValidate]
            public string Name { get; set; }
        }

        public class Update : DataDto
        {
            [DisplayName("tên")]
            [StringLengthValidate(100)]
            [RequiredValidate]
            public string Name { get; set; }
        }
    }
}
