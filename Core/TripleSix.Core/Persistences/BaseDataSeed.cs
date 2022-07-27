using Microsoft.EntityFrameworkCore;

namespace TripleSix.Core.Persistences
{
    /// <summary>
    /// Data Seeder phát sinh dữ liệu.
    /// </summary>
    public abstract class BaseDataSeed : IDataSeed
    {
        /// <inheritdoc/>
        public abstract void OnDataSeeding(ModelBuilder builder);
    }
}
