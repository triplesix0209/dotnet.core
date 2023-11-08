namespace Sample.Application.Dto.Admins
{
    [MapFromEntity(typeof(Account))]
    public class AccountDataAdminDto : BaseDataAdminDto
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public SiteDataAdminDto Site { get; set; }
    }
}
