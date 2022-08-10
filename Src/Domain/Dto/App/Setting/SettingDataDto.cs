namespace Sample.Domain.Dto
{
    [MapFromEntity(typeof(Setting))]
    public class SettingDataDto : BaseDto
    {
        [DisplayName("mã số")]
        public string Code { get; set; }

        [DisplayName("giá trị")]
        public string Value { get; set; }

        [DisplayName("mô tả")]
        public string Description { get; set; }
    }
}
