using System.ComponentModel;
using TripleSix.Core.Dto;

namespace Sample.Common.Dto
{
    public class SettingFilterDto : FilterDto
    {
        [DisplayName("lọc theo mã số")]
        public string Code { get; set; }
    }
}
