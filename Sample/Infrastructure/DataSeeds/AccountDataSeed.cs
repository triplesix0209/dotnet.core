namespace Sample.Infrastructure.Seeds
{
    public class AccountDataSeed : BaseDataSeed
    {
        /// <inheritdoc/>
        public override void OnDataSeeding(ModelBuilder builder, DatabaseFacade database)
        {
            //var cmd = database.GetDbConnection().CreateCommand();
            //Console.WriteLine(cmd.Connection.State);

            var accounts = new List<Account>()
            {
                new Account
                {
                    Id = Guid.Parse("653dc4d4-ca05-45ac-83cd-e98fa91b890f"),
                    Code = "EM001",
                    Name = "Nhân Viên 2",
                    SiteId = Guid.Parse("7a2ed7c2-e6f7-48c1-a86a-aa701aee1e22"),
                },
                new Account
                {
                    Id = Guid.Parse("6f6e615e-feeb-40b5-b53c-7f9056082d36"),
                    Code = "EM002",
                    Name = "Nhân Viên 2",
                    SiteId = Guid.Parse("7a2ed7c2-e6f7-48c1-a86a-aa701aee1e22"),
                },
                new Account
                {
                    Id = Guid.Parse("72b44a93-defc-4e24-a466-0d0d36b3669c"),
                    Code = "EM003",
                    Name = "Nhân Viên 3",
                    SiteId = Guid.Parse("3e08cf2e-d8a2-49b5-8663-fa31f0cdd168"),
                },
            };

            builder.Entity<Account>().HasData(accounts);
        }
    }
}
