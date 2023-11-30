namespace Sample.Application.Services
{
    public interface ISiteService : IStrongService<Site>
    {
        Task RunJob(string data);
    }

    public class SiteService : StrongService<Site>, ISiteService
    {
        public SiteService(IDbDataContext db)
            : base(db)
        {
        }

        public IApplicationDbContext Db { get; set; }

        public async Task RunJob(string data)
        {
            throw new Exception(data);
        }
    }
}
