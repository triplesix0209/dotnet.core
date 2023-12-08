using Hangfire;
using TripleSix.Core.Hangfire;

namespace Sample.Application.Services
{
    public interface ISiteService : IStrongService<Site>
    {
        [DisplayName("Hello")]
        [Queue("sample")]
        Task RunJob(JobContext? jobContext);
    }

    public class SiteService : StrongService<Site>, ISiteService
    {
        public SiteService(IDbDataContext db)
            : base(db)
        {
        }

        public IApplicationDbContext Db { get; set; }

        public Task RunJob(JobContext? jobContext)
        {
            return Task.CompletedTask;
        }
    }
}
