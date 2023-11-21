namespace Sample.Application.Dto.Admins
{
    [MapData<Account, AccountDataAdminDto>]
    public class AccountDataAdminDto : BaseDataAdminDto
    {
        public string Code { get; set; }

        public string Name { get; set; }
    }
}
