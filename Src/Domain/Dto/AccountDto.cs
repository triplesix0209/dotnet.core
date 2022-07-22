namespace Sample.Domain.Dto
{
    [MapEntity(typeof(Account))]
    public class AccountDto : DataDto
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public DateTime? BirthDate { get; set; }
    }
}
