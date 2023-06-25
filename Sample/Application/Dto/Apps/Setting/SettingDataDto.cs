namespace Sample.Application.Dto.Apps
{
    [MapFromEntity(typeof(Setting))]
    public class SettingDataDto : BaseDto
    {
        public string Code { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }
    }
}
