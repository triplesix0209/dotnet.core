namespace Sample.Domain.Dto
{
    [MapFromEntity(typeof(PermissionValue))]
    public class PermissionValueDto : BaseDto
    {
        [DisplayName("mã số")]
        public string? Code { get; set; }

        [DisplayName("tên gọi")]
        public string? Name { get; set; }

        [DisplayName("tên nhóm")]
        public string? CategoryName { get; set; }

        [DisplayName("giá trị")]
        public PermissionValues Value { get; set; }

        [DisplayName("giá trị thực")]
        public bool ActualValue { get; set; }
    }
}
