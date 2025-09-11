namespace Sample.Application.Dto.Admins
{
    public class SiteCreateAdminDto : BaseInputDto<Site>
    {
        [Required]
        [MustTrim]
        [MustUpperCase]
        [MaxLength(100)]
        public string Code { get; set; }

        [Required]
        [MustTrim]
        [MaxLength(200)]
        public string Name { get; set; }
    }
}
