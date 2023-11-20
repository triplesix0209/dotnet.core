namespace Sample.Application.Dto.Admins
{
    [MapFromEntity(typeof(Account))]
    public class AccountDetailAdminDto : AccountDataAdminDto
    {
        public SiteDataAdminDto Site { get; set; }
    }
}
