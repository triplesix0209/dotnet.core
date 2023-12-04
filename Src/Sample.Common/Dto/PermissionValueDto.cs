using System.ComponentModel;
using Sample.Common.Enum;
using TripleSix.CoreOld.Dto;

namespace Sample.Common.Dto
{
    public class PermissionValueDto : DataDto
    {
        [DisplayName("mã số")]
        public string Code { get; set; }

        [DisplayName("tên gọi")]
        public string Name { get; set; }

        [DisplayName("tên nhóm")]
        public string CategoryName { get; set; }

        [DisplayName("giá trị")]
        public PermissionValues Value { get; set; }

        [DisplayName("giá trị thực")]
        public bool ActualValue { get; set; }
    }
}
