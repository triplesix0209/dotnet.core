namespace Sample.Domain.Enum
{
    public enum AccountLevels
    {
        Root = 0,

        [Description("quản trị")]
        Admin = 1,

        [Description("thông thường")]
        Common = 2,
    }
}
