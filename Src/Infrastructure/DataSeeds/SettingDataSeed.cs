namespace Sample.Infrastructure.Seeds
{
    public class SettingDataSeed : BaseDataSeed
    {
        public override void OnDataSeeding(ModelBuilder builder)
        {
            var data = new[]
            {
                new
                {
                    Id = Guid.Parse("92b35930-6354-44c8-bc4d-38194e631f98"),
                    Code = "mã thiết lập",
                    Value = "giá trị",
                    Description = "mô tả",
                },
            };

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
