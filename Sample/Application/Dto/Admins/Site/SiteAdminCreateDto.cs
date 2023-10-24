namespace Sample.Application.Dto.Admins
{
    [MapToEntity(typeof(Site))]
    public class SiteAdminCreateDto : BaseDto
    {
        [Required]
        [MustWordNumber]
        [MustUpperCase]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
