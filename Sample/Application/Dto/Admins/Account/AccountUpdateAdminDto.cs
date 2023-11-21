namespace Sample.Application.Dto.Admins
{
    [MapData<AccountUpdateAdminDto, Account>]
    public class AccountUpdateAdminDto : BaseDto
    {
        public string Name { get; set; }

        public Guid SiteId { get; set; }
    }
}
