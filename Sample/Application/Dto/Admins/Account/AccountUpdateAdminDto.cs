namespace Sample.Application.Dto.Admins
{
    public class AccountUpdateAdminDto : BaseInputDto<Account>
    {
        public string Name { get; set; }

        public Guid SiteId { get; set; }
    }
}
