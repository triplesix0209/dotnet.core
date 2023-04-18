namespace TripleSix.Core.AutoAdmin
{
    /// <summary>
    /// Cài đặt Admin cho controller.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AdminControllerAttribute : Attribute
    {
        /// <summary>
        /// Admin model sử dụng.
        /// </summary>
        public Type ModelType { get; set; }

        /// <summary>
        /// Bật/Tắt.
        /// </summary>
        public bool Enable { get; set; } = true;

        /// <summary>
        /// Ẩn/Hiện.
        /// </summary>
        public bool Render { get; set; } = true;

        /// <summary>
        /// Tên gọi.
        /// </summary>
        public string? Name { get; set; } = null;

        /// <summary>
        /// Icon nhóm (mdi-icon).
        /// </summary>
        public string? Icon { get; set; } = null;

        /// <summary>
        /// Mã nhóm.
        /// </summary>
        public string? GroupCode { get; set; } = null;

        /// <summary>
        /// Tên nhóm.
        /// </summary>
        public string? GroupName { get; set; } = null;

        /// <summary>
        /// Icon nhóm.
        /// </summary>
        public string? GroupIcon { get; set; } = null;

        /// <summary>
        /// Thứ tự hiển thị.
        /// </summary>
        public int DisplayOrder { get; set; } = 0;

        /// <summary>
        /// Bật/Tắt các phương thức xem dữ liệu.
        /// </summary>
        public bool EnableRead { get; set; } = true;

        /// <summary>
        /// Bật/Tắt các tính năng tạo dữ liệu.
        /// </summary>
        public bool EnableCreate { get; set; } = true;

        /// <summary>
        /// Bật/Tắt các tính năng sửa dữ liệu.
        /// </summary>
        public bool EnableUpdate { get; set; } = true;

        /// <summary>
        /// Bật/Tắt các tính năng xóa dữ liệu.
        /// </summary>
        public bool EnableDelete { get; set; } = true;

        /// <summary>
        /// Bật/Tắt các phương thức xem lịch sử thay đổi.
        /// </summary>
        public bool EnableChangeLog { get; set; } = true;

        /// <summary>
        /// Bật/Tắt các tính năng xuất dữ liệu.
        /// </summary>
        public bool EnableExport { get; set; } = true;
    }
}
