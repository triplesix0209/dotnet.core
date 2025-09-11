namespace Sample.Application.Dto.Admins
{
    public class SiteUpdateAdminDto : BaseInputDto<Site>
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
