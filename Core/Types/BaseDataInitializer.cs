namespace TripleSix.Core.Types
{
    /// <summary>
    /// Lớp cơ sở cho các DataInitializer.
    /// </summary>
    public abstract class BaseDataInitializer
    {
        /// <summary>
        /// Khởi tạo dữ liệu.
        /// </summary>
        /// <returns> Một tác vụ đại diện cho hoạt động khởi tạo. </returns>
        public abstract Task Init();
    }
}
