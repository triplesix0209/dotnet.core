namespace Sample.Domain.Dto
{
    public class AccountDto : DataDto
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public DateTime? BirthDate { get; set; }
    }
}
