using System.ComponentModel;
using TripleSix.CoreOld.Dto;

namespace Sample.Common.Dto
{
    public class SettingFilterDto : FilterDto
    {
        [DisplayName("lọc theo mã số")]
        public string Code { get; set; }
    }
}
