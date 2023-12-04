using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sample.Common.Enum;

namespace Sample.Data.Entities
{
    public class AccountEntity : ModelEntity<AccountEntity>
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        public bool IsEmailVerified { get; set; }

        public string AvatarLink { get; set; }

        public AccountLevels AccessLevel { get; set; }

        public Guid? PermissionGroupId { get; set; }

        [ForeignKey(nameof(PermissionGroupId))]
        public virtual PermissionGroupEntity PermissionGroup { get; set; }

        public virtual IList<AccountAuthEntity> Auths { get; set; }

        public virtual IList<AccountSessionEntity> Sessions { get; set; }

        public virtual IList<AccountVerifyEntity> Verifies { get; set; }

        protected override void ModelConfigure(EntityTypeBuilder<AccountEntity> builder)
        {
            base.ModelConfigure(builder);

            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.AccessLevel);
        }
    }
}
