namespace Sample.Infrastructure.Seeds
{
    public class AccountDataSeed : BaseDataSeed
    {
        public override void OnDataSeeding(ModelBuilder builder, DatabaseFacade database)
        {
            var accounts = new List<Account>()
            {
                new Account
                {
                    Id = Guid.Parse("653dc4d4-ca05-45ac-83cd-e98fa91b890f"),
                    Code = "root",
                    Name = "Root",
                },
            };

            builder.Entity<Account>().HasData(accounts);
        }
    }
}
