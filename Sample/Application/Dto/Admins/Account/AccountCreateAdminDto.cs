namespace Sample.Application.Dto.Admins
{
    [MapData<AccountCreateAdminDto, Account>]
    public class AccountCreateAdminDto : BaseDto
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
