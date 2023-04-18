namespace Sample.Infrastructure.Seeds
{
    public class SettingDataSeed : BaseDataSeed
    {
        public override void OnDataSeeding(ModelBuilder builder)
        {
            var instance = new DbSettings();
            var settings = typeof(DbSettings).GetProperties()
                .Select(property =>
                {
                    var item = property.GetValue(instance) as DbSettingItem;
                    return new Setting
                    {
                        Id = item!.Id,
                        Code = property.Name,
                        Value = item.DefaultValue,
                        Description = item.Description,
                    };
                });

            builder.Entity<Setting>().HasData(settings);
        }
    }
}
