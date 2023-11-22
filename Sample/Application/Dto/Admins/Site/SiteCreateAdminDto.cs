namespace Sample.Application.Dto.Admins
{
    [MapToEntity<Site>]
    public class SiteCreateAdminDto : BaseDto
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
