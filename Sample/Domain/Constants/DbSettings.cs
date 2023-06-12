namespace Sample.Domain.Constants
{
    public class DbSettings
    {
        public DbSettingItem SessionLifetime => new(Guid.Parse("22ccd8c5-6656-48f5-a73e-8d75895c9adc"))
        {
            Description = "Thời gian sống của session (phút)",
            DefaultValue = "240",
        };
    }
}
