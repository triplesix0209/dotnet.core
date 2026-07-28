namespace TripleSix.Core.Exceptions
{
    /// <summary>
    /// Lỗi job, dùng để skip retry.
    /// </summary>
    public class JobException : BaseException
    {
        /// <summary>
        /// Khởi tạo <see cref="JobException"/>.
        /// </summary>
        /// <param name="msg">Thông báo lỗi.</param>
        public JobException(string msg)
            : base(msg)
        {
        }
    }
}
