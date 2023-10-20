using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace TripleSix.Core.DataContext
{
    /// <summary>
    /// Interface seed dữ liệu.
    /// </summary>
    public interface IDbDataSeed
    {
        /// <summary>
        /// Hàm phát sinh dữ liệu.
        /// </summary>
        /// <param name="builder"><see cref="ModelBuilder"/>.</param>
        /// <param name="database"><see cref="DatabaseFacade"/>.</param>
        void OnDataSeeding(ModelBuilder builder, DatabaseFacade database);
    }
}
