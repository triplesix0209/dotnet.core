using Microsoft.EntityFrameworkCore;
using TripleSix.Core.DataContext;

namespace TripleSix.Core.AutoAdmin
{
    public interface IObjectLogDbContext : IDbDataContext
    {
        DbSet<ObjectLog> ObjectLog { get; set; }

        DbSet<ObjectLogField> ObjectLogField { get; set; }
    }
}
