using System.ComponentModel;

namespace Sample.Common.Enum
{
    public enum PermissionValues
    {
        [Description("kế thừa")]
        Inherit = 0,

        [Description("cho phép")]
        Allow = 1,

        [Description("cấm")]
        Deny = -1,
    }
}
