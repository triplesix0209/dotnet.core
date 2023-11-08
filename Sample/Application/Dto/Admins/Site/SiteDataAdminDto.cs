namespace Sample.Application.Dto.Admins
{
    [MapFromEntity(typeof(Site))]
    public class SiteDataAdminDto : BaseDataAdminDto
    {
        public string Code { get; set; }

        public string Name { get; set; }
    }
}
