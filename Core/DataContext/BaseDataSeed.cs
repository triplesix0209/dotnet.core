using Microsoft.EntityFrameworkCore;

namespace TripleSix.Core.DataContext
{
    /// <summary>
    /// Data Seeder phát sinh dữ liệu.
    /// </summary>
    public abstract class BaseDataSeed : IDbDataSeed
    {
        public abstract void OnDataSeeding(ModelBuilder builder);
    }
}
