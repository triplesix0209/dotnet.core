namespace TripleSix.Core.AutoAdmin
{
    /// <summary>
    /// Phân loại phương thức của admin.
    /// </summary>
    public enum AdminMethodTypes
    {
        /// <summary>
        /// Danh sách.
        /// </summary>
        List = 1,

        /// <summary>
        /// Chi tiết.
        /// </summary>
        Detail = 2,

        /// <summary>
        /// Tạo
        /// </summary>
        Create = 3,

        /// <summary>
        /// Sửa.
        /// </summary>
        Update = 4,

        /// <summary>
        /// Xóa.
        /// </summary>
        Delete = 5,

        /// <summary>
        /// Phục hồi.
        /// </summary>
        Restore = 6,

        /// <summary>
        /// Danh sách lịch sử thay đổi.
        /// </summary>
        ListChangeLog = 7,

        /// <summary>
        /// Chi tiết thay đổi
        /// </summary>
        DetailChangeLog = 8,

        /// <summary>
        /// Xuất dữ liệu.
        /// </summary>
        Export = 9,
    }
}
