namespace Sample.Application.Dto.Admins
{
    [MapData<Site, SiteDataAdminDto>]
    public class SiteDataAdminDto : BaseDataAdminDto
    {
        public string Code { get; set; }

        public string Name { get; set; }
    }
}
