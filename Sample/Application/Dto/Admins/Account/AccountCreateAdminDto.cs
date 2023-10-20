namespace Sample.Application.Dto.Admins
{
    [MapToEntity(typeof(Account))]
    public class AccountCreateAdminDto : BaseDto
    {
        [Required]
        public string Code { get; set; }

        //[Required]
        //public string Name { get; set; }
    }
}
