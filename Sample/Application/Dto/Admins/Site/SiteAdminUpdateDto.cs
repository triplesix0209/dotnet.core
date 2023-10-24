namespace Sample.Application.Dto.Admins
{
    [MapToEntity(typeof(Site))]
    public class SiteAdminUpdateDto : BaseDto
    {
        public string Code { get; set; }

        public string Name { get; set; }
    }
}
