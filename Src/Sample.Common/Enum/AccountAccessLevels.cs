﻿using System.ComponentModel;

namespace Sample.Common.Enum
{
    public enum AccountAccessLevels
    {
        [Description("thông thường")]
        Common = 0,

        [Description("quản trị")]
        Admin = 1,

        [Description("root")]
        Root = 2,
    }
}
