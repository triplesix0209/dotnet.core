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
        /// Chi tiết sửa.
        /// </summary>
        UpdateView = 4,

        /// <summary>
        /// Sửa.
        /// </summary>
        Update = 5,

        /// <summary>
        /// Tạm xóa.
        /// </summary>
        SoftDelete = 6,

        /// <summary>
        /// Phục hồi.
        /// </summary>
        Restore = 7,

        /// <summary>
        /// Danh sách lịch sử thay đổi.
        /// </summary>
        ListChangeLog = 8,

        /// <summary>
        /// Chi tiết thay đổi.
        /// </summary>
        DetailChangeLog = 9,

        /// <summary>
        /// Xuất dữ liệu.
        /// </summary>
        Export = 10,
    }
}
