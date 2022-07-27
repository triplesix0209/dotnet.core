using Microsoft.EntityFrameworkCore;

namespace TripleSix.Core.Persistences
{
    /// <summary>
    /// Interface seed dữ liệu.
    /// </summary>
    public interface IDataSeed
    {
        /// <summary>
        /// Hàm phát sinh dữ liệu.
        /// </summary>
        /// <param name="builder"><see cref="ModelBuilder"/>.</param>
        void OnDataSeeding(ModelBuilder builder);
    }
}
