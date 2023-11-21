namespace Sample.Infrastructure.Seeds
{
    public class SiteDataSeed : BaseDataSeed
    {
        /// <inheritdoc/>
        public override void OnDataSeeding(ModelBuilder builder, DatabaseFacade database)
        {
            var sites = new List<Site>()
            {
                new Site
                {
                    Id = Guid.Parse("7a2ed7c2-e6f7-48c1-a86a-aa701aee1e22"),
                    Code = "H001",
                    Name = "Quận 5",
                },
                new Site
                {
                    Id = Guid.Parse("3e08cf2e-d8a2-49b5-8663-fa31f0cdd168"),
                    Code = "H002",
                    Name = "Quận 6",
                },
            };

            builder.Entity<Site>().HasData(sites);
        }
    }
}
