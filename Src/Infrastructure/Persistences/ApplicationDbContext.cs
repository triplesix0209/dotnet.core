using Microsoft.EntityFrameworkCore;
using TripleSix.Core.Persistences;

namespace Sample.Infrastructure.Persistences
{
    public partial class ApplicationDbContext : BaseDbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}
