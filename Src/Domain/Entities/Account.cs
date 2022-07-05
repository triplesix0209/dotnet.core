using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sample.Domain.Entities
{
    /// <summary>
    /// Tài khoản.
    /// </summary>
    public class Account : StrongEntity<Account>
    {
        /// <summary>
        /// Tên gọi.
        /// </summary>
        [Required]
        public string? Name { get; set; }

        /// <summary>
        /// Ngày sinh.
        /// </summary>
        public DateTime? BirthDate { get; set; }

        public override void Configure(EntityTypeBuilder<Account> builder)
        {
            base.Configure(builder);

            builder.HasIndex(x => x.Name);
        }
    }
}
