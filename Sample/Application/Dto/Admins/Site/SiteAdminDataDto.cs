namespace Sample.Application.Dto.Admins
{
    [MapFromEntity(typeof(Site))]
    public class SiteAdminDataDto : BaseAdminDataDto
    {
        public string Code { get; set; }

        public string Name { get; set; }
    }
}
