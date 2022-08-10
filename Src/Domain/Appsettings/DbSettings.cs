namespace Sample.Domain.Appsettings
{
    public class DbSettings
    {
        public DbSettingItem AccountRegisterAutoAccept => new()
        {
            Id = Guid.Parse("22ccd8c5-6656-48f5-a73e-8d75895c9adc"),
            Description = "Tự động duyệt các tài khoản mới đăng ký",
            DefaultValue = "false",
        };
    }

    public class DbSettingItem
    {
        public Guid Id { get; set; }

        public string? Description { get; set; }

        public string? DefaultValue { get; set; }
    }
}
