namespace TripleSix.Core.AutoAdmin
{
    /// <summary>
    /// Cài đặt thông tin Admin cho property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AdminPropertyAttribute : Attribute
    {
        /// <summary>
        /// Loại dữ liệu.
        /// </summary>
        public AdminFieldTypes Type { get; set; } = 0;

        /// <summary>
        /// Ẩn/hiện.
        /// </summary>
        public bool Render { get; set; } = true;

        /// <summary>
        /// Mã nhóm.
        /// </summary>
        public string? GroupCode { get; set; } = null;

        /// <summary>
        /// Tên nhóm.
        /// </summary>
        public string? GroupName { get; set; } = null;

        /// <summary>
        /// Icon nhóm (mdi-icon).
        /// </summary>
        public string? GroupIcon { get; set; } = null;

        /// <summary>
        /// Tên field được sử dụng để hiển thị.
        /// </summary>
        public string? DisplayField { get; set; } = null;

        /// <summary>
        /// Field được sử dụng để sort.
        /// </summary>
        public string? SortField { get; set; } = null;

        /// <summary>
        /// Là key field của model.
        /// </summary>
        public bool IsModelKey { get; set; } = false;

        /// <summary>
        /// Là field hiển thị của model.
        /// </summary>
        public bool IsModelText { get; set; } = false;

        /// <summary>
        /// Số col chiếm ở grid.
        /// </summary>
        public int? GridCol { get; set; } = null;

        /// <summary>
        /// Dto đối tượng liên kết.
        /// </summary>
        public Type? LinkModelType { get; set; } = null;
    }
}
