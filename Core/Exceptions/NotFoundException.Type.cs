namespace TripleSix.Core.Exceptions
{
    /// <summary>
    /// Lỗi không tìm thấy entity.
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu.</typeparam>
    public class NotFoundException<T> : NotFoundException
    {
        /// <summary>
        /// Khởi tạo <see cref="NotFoundException"/>.
        /// </summary>
        public NotFoundException()
            : base(typeof(T))
        {
        }
    }
}
