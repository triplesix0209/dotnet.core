namespace Sample.Application.Dto.Admins
{
    [MapToEntity(typeof(Account))]
    public class AccountAdminUpdateDto : BaseDto
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public Guid SiteId { get; set; }
    }
}
