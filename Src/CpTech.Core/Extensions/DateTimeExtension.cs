using System;
using CpTech.Core.Helpers;

namespace CpTech.Core.Extensions
{
    public static class DateTimeExtension
    {
        public static long ToEpochTimestamp(this DateTime datetime)
        {
            return (long)(datetime - DateTimeHelper.Epoch)
                .TotalMilliseconds;
        }
    }
}