using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sample.Common.Enum;

namespace Sample.Data.Entities
{
    public class AccountVerifyEntity : ModelEntity<AccountVerifyEntity>
    {
        public AccountVerifyTypes Type { get; set; }

        public DateTime ExpiryDatetime { get; set; }

        [Required]
        public Guid AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public virtual AccountEntity Account { get; set; }

        protected override void ModelConfigure(EntityTypeBuilder<AccountVerifyEntity> builder)
        {
            base.ModelConfigure(builder);

            builder.HasIndex(x => x.Type);
            builder.HasIndex(x => x.ExpiryDatetime);
        }
    }
}
