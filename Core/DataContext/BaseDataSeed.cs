using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace TripleSix.Core.DataContext
{
    /// <summary>
    /// Data Seeder phát sinh dữ liệu.
    /// </summary>
    public abstract class BaseDataSeed : IDbDataSeed
    {
        /// <inheritdoc/>
        public abstract void OnDataSeeding(ModelBuilder builder, DatabaseFacade database);
    }
}
