namespace Sample.Application.Dto.Admins
{
    [MapFromEntity(typeof(Account))]
    public class AccountAdminDataDto : BaseAdminDataDto
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public SiteAdminDataDto Site { get; set; }
    }
}
