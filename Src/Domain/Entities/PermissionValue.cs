namespace Sample.Domain.Entities
{
    public class PermissionValue : BaseEntity<PermissionValue>
    {
        [Required]
        [MaxLength(100)]
        public string Code { get; set; }

        public Guid GroupId { get; set; }

        public PermissionValues Value { get; set; }

        public bool ActualValue { get; set; }

        [ForeignKey(nameof(Code))]
        public virtual Permission Permission { get; set; }

        [ForeignKey(nameof(GroupId))]
        public virtual PermissionGroup PermissionGroup { get; set; }

        public override void Configure(EntityTypeBuilder<PermissionValue> builder)
        {
            builder.HasKey(x => new { x.Code, x.GroupId });
        }
    }
}
