namespace Sample.Domain.Dto
{
    public class PermissionValueDto : PermissionItemDto
    {
        [DisplayName("giá trị")]
        public PermissionValues Value { get; set; }

        [DisplayName("giá trị thực")]
        public bool ActualValue { get; set; }
    }
}
