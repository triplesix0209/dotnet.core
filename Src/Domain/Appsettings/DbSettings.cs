namespace Sample.Domain.Appsettings
{
    public class DbSettings
    {
        public DbSettingItem AccountRegisterAutoAccept => new()
        {
            Description = "Tự động duyệt các tài khoản mới đăng ký",
            DefaultValue = "false",
        };
    }

    public class DbSettingItem
    {
        public string? Description { get; set; }

        public string? DefaultValue { get; set; }
    }
}
