using System;

namespace CpTech.Core.Helpers
{
    public static class DateTimeHelper
    {
        internal static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime ParseEpochTimestamp(long timestamp)
        {
            return Epoch.AddMilliseconds(timestamp);
        }
    }
}