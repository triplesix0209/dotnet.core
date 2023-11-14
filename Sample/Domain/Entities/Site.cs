﻿namespace Sample.Domain.Entities
{
    [Comment("Chi nhánh")]
    public class Site : StrongEntity<Site>,
        ISapSynchronizableEntity
    {
        [Comment("Mã số")]
        [MaxLength(100)]
        public string Code { get; set; }

        [Comment("Tên gọi")]
        [MaxLength(200)]
        public string Name { get; set; }

        [Comment("Mã SAP")]
        [MaxLength(100)]
        public string SapCode { get; set; }

        public virtual IList<Account> Accounts { get; set; }

        public override void Configure(EntityTypeBuilder<Site> builder)
        {
            base.Configure(builder);

            builder.HasIndex(x => x.Code).IsUnique();
            builder.HasIndex(x => x.Name);
        }
    }
}
