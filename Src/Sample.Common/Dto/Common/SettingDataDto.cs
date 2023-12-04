using System.ComponentModel;

namespace Sample.Common.Dto
{
    public class SettingDataDto : DataDto
    {
        [DisplayName("mã số")]
        public string Code { get; set; }

        [DisplayName("giá trị")]
        public string Value { get; set; }

        [DisplayName("mô tả")]
        public string Description { get; set; }
    }
}
