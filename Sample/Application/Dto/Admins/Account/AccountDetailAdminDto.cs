namespace Sample.Application.Dto.Admins
{
    [MapData<Account, AccountDetailAdminDto>]
    public class AccountDetailAdminDto : AccountDataAdminDto
    {
        public SiteDataAdminDto Site { get; set; }
    }
}
