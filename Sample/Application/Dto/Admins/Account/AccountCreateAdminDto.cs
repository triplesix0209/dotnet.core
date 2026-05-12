
namespace Sample.Application.Dto.Admins
{
    public class AccountCreateAdminDto : BaseInputDto<Account>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public Guid SiteId { get; set; }

        public override async Task<Account> ToEntity(IServiceProvider serviceProvider, Account? source = null)
        {
            var result = await base.ToEntity(serviceProvider, source);
            result.Code = RandomHelper.RandomString(10);
            return result;
        }
    }
}
