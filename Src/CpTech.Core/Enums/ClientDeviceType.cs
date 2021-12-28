using System.ComponentModel;

namespace CpTech.Core.Enums
{
    public enum ClientDeviceType
    {
        [Description("Không xác định")]
        Unknown = 0,

        [Description("Web")]
        Web = 1,

        [Description("Android")]
        Android = 2,

        [Description("iOS")]
        IOS = 3,
    }
}
