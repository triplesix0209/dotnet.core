using Microsoft.EntityFrameworkCore;
using TripleSix.Core.Persistences;

namespace Sample.Domain.Persistences
{
    /// <summary>
    /// Interface DbContext của ứng dụng.
    /// </summary>
    public interface IApplicationDbContext : IDbDataContext
    {
        DbSet<Account>? Account { get; set; }
    }
}
