using Microsoft.EntityFrameworkCore;
using TripleSix.Core.Persistences;

namespace TripleSix.Core.AutoAdmin
{
    public interface IObjectLogDbContext : IDbDataContext
    {
        DbSet<ObjectLog> ObjectLog { get; set; }

        DbSet<ObjectLogField> ObjectLogField { get; set; }
    }
}
