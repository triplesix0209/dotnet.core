using System.ComponentModel;

namespace TripleSix.Core.Types
{
    /// <summary>
    /// Sự kiện thay đổi của entity.
    /// </summary>
    public enum EntityEvents
    {
        /// <summary>
        /// Tạo mới.
        /// </summary>
        [Description("Tạo mới")]
        Created = 1,

        /// <summary>
        /// Chỉnh sửa.
        /// </summary>
        [Description("Chỉnh sửa")]
        Updated = 2,

        /// <summary>
        /// Xóa.
        /// </summary>
        [Description("Xóa")]
        HardDeleted = 3,

        /// <summary>
        /// Khóa.
        /// </summary>
        [Description("Khóa")]
        SoftDeleted = 4,

        /// <summary>
        /// Mở khóa.
        /// </summary>
        [Description("Mở khóa")]
        Restore = 5,
    }
}
