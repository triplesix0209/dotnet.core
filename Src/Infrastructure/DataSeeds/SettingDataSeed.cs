using Sample.Domain.Appsettings;

namespace Sample.Infrastructure.Seeds
{
    public class SettingDataSeed : BaseDataSeed
    {
        public override void OnDataSeeding(ModelBuilder builder)
        {
            var instance = new DbSettings();
            var data = instance.GetType().GetProperties()
                .Select(property =>
                {
                    var item = property.GetValue(instance) as DbSettingItem;
                    return new
                    {
                        Id = item!.Id,
                        Code = property.Name,
                        Value = item!.DefaultValue,
                        Description = item!.Description,
                    };
                });

            var settings = data.Select(x => new Setting
            {
                Id = x.Id,
                Code = x.Code,
                Value = x.Value,
                Description = x.Description,
            });
            builder.Entity<Setting>().HasData(settings);
        }
    }
}
