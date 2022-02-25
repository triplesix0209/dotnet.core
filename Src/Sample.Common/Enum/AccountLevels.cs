﻿using System.ComponentModel;

namespace Sample.Common.Enum
{
    public enum AccountLevels
    {
        [Description("root")]
        Root = 0,

        [Description("quản trị")]
        Admin = 1,

        [Description("thông thường")]
        Common = 2,
    }
}