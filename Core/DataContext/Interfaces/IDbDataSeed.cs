using Microsoft.EntityFrameworkCore;

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
        void OnDataSeeding(ModelBuilder builder);
    }
}
