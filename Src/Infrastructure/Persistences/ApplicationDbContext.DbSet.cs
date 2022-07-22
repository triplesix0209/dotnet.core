using Microsoft.EntityFrameworkCore;
using Sample.Domain.Entities;
using Sample.Domain.Persistences;

namespace Sample.Infrastructure.Persistences
{
    public partial class ApplicationDbContext : IApplicationDbContext
    {
        public DbSet<Account> Account { get; set; }
    }
}
