namespace TripleSix.Core.Helpers
{
    /// <summary>
    /// Helper xử lý ngày tháng.
    /// </summary>
    public static partial class DateTimeHelper
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Chuyển ngày giờ sang epoch timestamp.
        /// </summary>
        /// <param name="datetime">Dữ liệu cần xử lý.</param>
        /// <returns>Số expoch timestamp.</returns>
        public static long ToEpochTimestamp(this DateTime datetime)
        {
            return (long)(datetime - Epoch).TotalMilliseconds;
        }

        /// <summary>
        /// Chuyển epoch timestamp sang ngày giờ.
        /// </summary>
        /// <param name="timestamp">Dử liệu cần chuyển đổi.</param>
        /// <returns>Dữ liệu DateTime.</returns>
        public static DateTime ToDateTime(this long timestamp)
        {
            return Epoch.AddMilliseconds(timestamp);
        }
    }
}