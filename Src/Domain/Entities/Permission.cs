namespace Sample.Domain.Entities
{
    public class Permission : BaseEntity<Permission>
    {
        [Key]
        [MaxLength(100)]
        public string? Code { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string? CategoryName { get; set; }

        public virtual List<PermissionValue>? PermissionValues { get; set; }

        public override void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasIndex(x => x.Code);
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.CategoryName);
        }
    }
}
