namespace Sample.Application.Dto.Admins
{
    [MapToEntity<Account>(true)]
    public class AccountUpdateAdminDto : BaseDto
    {
        public string Name { get; set; }

        public Guid SiteId { get; set; }
    }
}
