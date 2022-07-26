namespace Sample.Domain.Dto
{
    [MapEntity(typeof(Account), ignoreUnmapedProperties: true)]
    public class AccountDto : DataDto
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? Username { get; set; }
    }
}
