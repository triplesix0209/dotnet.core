namespace Sample.Domain.Dto
{
    [MapFromEntity(typeof(Permission))]
    public class PermissionItemDto : BaseDto
    {
        [DisplayName("mã quyền")]
        public string Code { get; set; }

        [DisplayName("tên quyền")]
        public string Name { get; set; }

        [DisplayName("nhóm quyền")]
        public string CategoryName { get; set; }
    }
}
