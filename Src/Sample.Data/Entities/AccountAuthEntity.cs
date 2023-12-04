using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sample.Common.Enum;
using TripleSix.CoreOld.Entities;

namespace Sample.Data.Entities
{
    public class AccountAuthEntity : ModelEntity<AccountAuthEntity>
    {
        public AccountAuthTypes Type { get; set; }

        [MaxLength(100)]
        public string Username { get; set; }

        [MaxLength(100)]
        public string HashPasswordKey { get; set; }

        [MaxLength(128)]
        public string HashedPassword { get; set; }

        public Guid AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public virtual AccountEntity Account { get; set; }

        protected override void ModelConfigure(EntityTypeBuilder<AccountAuthEntity> builder)
        {
            base.ModelConfigure(builder);

            builder.HasIndex(x => x.Username).IsUnique();
        }
    }
}
