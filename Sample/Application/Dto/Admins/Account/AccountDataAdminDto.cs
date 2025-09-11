namespace Sample.Application.Dto.Admins
{
    public class AccountDataAdminDto : BaseDataAdminDto<Account>
    {
        public string Code { get; set; }

        public string Name { get; set; }
    }
}
