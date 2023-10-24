namespace Sample.Application.Dto.Admins
{
    [MapToEntity(typeof(Account))]
    public class AccountAdminCreateDto : BaseDto
    {
        [Required]
        [MustWordNumber]
        [MustUpperCase]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public Guid SiteId { get; set; }
    }
}
