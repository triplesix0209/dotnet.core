namespace Sample.Domain.Types
{
    public class DbSettingItem
    {
        public DbSettingItem(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }

        public string? Description { get; set; }

        public string? DefaultValue { get; set; }
    }
}
