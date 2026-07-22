namespace TripleSix.Core.Exceptions
{
    /// <summary>
    /// Lỗi hệ thống.
    /// </summary>
    public class AppException : BaseException
    {
        /// <summary>
        /// Khởi tạo <see cref="AppException"/>.
        /// </summary>
        /// <param name="msg">Thông báo lỗi.</param>
        /// <param name="status">Mã trạng thái HTTP.</param>
        public AppException(string msg, int status = 500)
            : base(msg)
        {
            HttpCodeStatus = status;
        }
    }
}
