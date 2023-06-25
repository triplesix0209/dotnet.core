namespace Sample.Application.Dto.Admins
{
    [MapToEntity(typeof(Account))]
    public class AccountUpdateAdminDto : BaseDto
    {
        public string Code { get; set; }

        public string Name { get; set; }
    }
}
