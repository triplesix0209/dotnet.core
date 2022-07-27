namespace Sample.Domain.Entities
{
    public class PermissionGroup : HierarchyEntity<PermissionGroup>
    {
        [Required]
        [MaxLength(100)]
        public override string? Code { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public virtual List<Account> Accounts { get; set; }

        public virtual List<PermissionValue> PermissionValues { get; set; }

        public override void Configure(EntityTypeBuilder<PermissionGroup> builder)
        {
            base.Configure(builder);

            builder.HasIndex(x => x.Name);
        }
    }
}
