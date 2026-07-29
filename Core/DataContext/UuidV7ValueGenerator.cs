using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace TripleSix.Core.DataContext
{
    /// <summary>
    /// Value generator tạo UUID V7 cho primary key.
    /// Riêng SQL Server sẽ sinh UUID V8 với timestamp nằm ở 6 byte cuối,
    /// do SQL Server so sánh uniqueidentifier với 6 byte cuối là nhóm có trọng số cao nhất.
    /// </summary>
    public class UuidV7ValueGenerator : ValueGenerator<Guid>
    {
        /// <inheritdoc/>
        public override bool GeneratesTemporaryValues => false;

        /// <inheritdoc/>
        public override Guid Next(EntityEntry entry)
        {
            var providerName = entry.Context.Database.ProviderName;
            return providerName != null && providerName.Contains("SqlServer", StringComparison.OrdinalIgnoreCase)
                ? NextSqlServer()
                : Guid.CreateVersion7();
        }

        private static Guid NextSqlServer()
        {
            Span<byte> bytes = stackalloc byte[16];
            RandomNumberGenerator.Fill(bytes[..10]);

            // version 8 (custom layout theo RFC 9562) + variant bits
            bytes[6] = (byte)((bytes[6] & 0x0F) | 0x80);
            bytes[8] = (byte)((bytes[8] & 0x3F) | 0x80);

            // unix timestamp millisecond 48-bit big-endian tại bytes 10..15,
            // nhóm byte SQL Server sort với trọng số cao nhất (cùng nguyên lý NEWSEQUENTIALID)
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            bytes[10] = (byte)(timestamp >> 40);
            bytes[11] = (byte)(timestamp >> 32);
            bytes[12] = (byte)(timestamp >> 24);
            bytes[13] = (byte)(timestamp >> 16);
            bytes[14] = (byte)(timestamp >> 8);
            bytes[15] = (byte)timestamp;

            return new Guid(bytes, bigEndian: true);
        }
    }
}
