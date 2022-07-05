using Microsoft.EntityFrameworkCore;
using Sample.Domain.Entities;

namespace Sample.Infrastructure.Persistences
{
    public partial class ApplicationDbContext
    {
        public DbSet<Account>? Account { get; set; }
    }
}
