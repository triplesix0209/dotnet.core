namespace Sample.Application.Services
{
    public interface ISiteService : IStrongService<Site>
    {
        [DisplayName("Hello")]
        Task RunJob(string text, int number, DateTime date);
    }

    public class SiteService : StrongService<Site>, ISiteService
    {
        public SiteService(IDbDataContext db)
            : base(db)
        {
        }

        public IApplicationDbContext Db { get; set; }

        public Task RunJob(string text, int number, DateTime date)
        {
            return Task.CompletedTask;
        }
    }
}
