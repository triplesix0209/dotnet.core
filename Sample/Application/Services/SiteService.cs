namespace Sample.Application.Services
{
    public interface ISiteService : IStrongService<Site>
    {
        Task RunJob();
    }

    public class SiteService : StrongService<Site>, ISiteService
    {
        public SiteService(IDbDataContext db)
            : base(db)
        {
        }

        public IApplicationDbContext Db { get; set; }

        public Task RunJob()
        {
            throw new NotImplementedException();
        }
    }
}
