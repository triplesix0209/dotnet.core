using System;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Extensions
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