namespace Sample.Application.Dto.Admins
{
    [MapToEntity<Account, AccountCreateAdminMapAction>(nameof(Account.Code))]
    public class AccountCreateAdminDto : BaseDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public Guid SiteId { get; set; }
    }

    public class AccountCreateAdminMapAction : IMappingAction<AccountCreateAdminDto, Account>
    {
        public void Process(AccountCreateAdminDto source, Account destination, ResolutionContext context)
        {
            destination.Code = RandomHelper.RandomString(10);
        }
    }
}
