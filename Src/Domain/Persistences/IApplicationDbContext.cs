using Microsoft.EntityFrameworkCore;
using TripleSix.Core.Persistences;

namespace Sample.Domain.Persistences
{
    public interface IApplicationDbContext : IDbDataContext
    {
        DbSet<Account> Account { get; set; }
    }
}
