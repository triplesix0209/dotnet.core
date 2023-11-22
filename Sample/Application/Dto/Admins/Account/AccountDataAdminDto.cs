namespace Sample.Application.Dto.Admins
{
    [MapFromEntity<Account>]
    public class AccountDataAdminDto : BaseDataAdminDto
    {
        public string Code { get; set; }

        public string Name { get; set; }
    }
}
