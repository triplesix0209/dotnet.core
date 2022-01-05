namespace TripleSix.AutoAdmin.Enums
{
    /// <summary>
    /// phân loại phương thức của admin.
    /// </summary>
    public enum AdminMethodTypes
    {
        // danh sách
        List = 1,

        // chi tiết
        Detail = 2,

        // tạo
        Create = 3,

        // sửa
        Update = 4,

        // xóa
        Delete = 5,

        // phục hồi
        Restore = 6,

        // danh sách lịch sử thay đổi
        ListChangeLog = 7,

        // chi tiết thay đổi
        DetailChangeLog = 8,

        // xuất dữ liệu
        Export = 9,
    }
}