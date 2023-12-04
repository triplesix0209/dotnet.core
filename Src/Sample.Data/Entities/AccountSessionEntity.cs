using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TripleSix.CoreOld.Entities;

namespace Sample.Data.Entities
{
    public class AccountSessionEntity : ModelEntity<AccountSessionEntity>
    {
        [Required]
        public override string Code { get; set; }

        public DateTime ExpiryDatetime { get; set; }

        public Guid AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public virtual AccountEntity Account { get; set; }

        protected override void ModelConfigure(EntityTypeBuilder<AccountSessionEntity> builder)
        {
            base.ModelConfigure(builder);

            builder.HasIndex(x => x.ExpiryDatetime);
        }
    }
}
