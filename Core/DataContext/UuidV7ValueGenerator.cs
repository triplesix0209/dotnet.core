using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace TripleSix.Core.DataContext
{
    /// <summary>
    /// Value generator tạo UUID V7 cho primary key.
    /// </summary>
    public class UuidV7ValueGenerator : ValueGenerator<Guid>
    {
        /// <inheritdoc/>
        public override bool GeneratesTemporaryValues => false;

        /// <inheritdoc/>
        public override Guid Next(EntityEntry entry)
        {
            return Guid.CreateVersion7();
        }
    }
}
