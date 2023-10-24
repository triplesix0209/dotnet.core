namespace Sample.Application.Services
{
    public interface ISiteService : IStrongService<Site>
    {
    }

    public class SiteService : StrongService<Site>, ISiteService
    {
        public SiteService(IDbDataContext db)
            : base(db)
        {
        }

        public IApplicationDbContext Db { get; set; }
    }
}
