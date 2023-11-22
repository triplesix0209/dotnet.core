namespace Sample.Application.Dto.Admins
{
    [MapToEntity<Site>(true)]
    public class SiteUpdateAdminDto : BaseDto
    {
        [MustTrim]
        [MustUpperCase]
        [MaxLength(100)]
        public string Code { get; set; }

        [MustTrim]
        [MaxLength(200)]
        public string Name { get; set; }
    }
}
