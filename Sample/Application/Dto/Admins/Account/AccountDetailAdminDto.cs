namespace Sample.Application.Dto.Admins
{
    [MapFromEntity<Account>]
    public class AccountDetailAdminDto : AccountDataAdminDto
    {
        public SiteDataAdminDto Site { get; set; }
    }
}
