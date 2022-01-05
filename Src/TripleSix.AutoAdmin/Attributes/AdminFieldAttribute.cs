using System;
using TripleSix.AutoAdmin.Enums;

namespace TripleSix.AutoAdmin.Attributes
{
    /// <summary>
    /// cấu hình auto admin field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AdminFieldAttribute : Attribute
    {
        /// <summary>
        /// loại field.
        /// </summary>
        public AdminFieldTypes? Type { get; set; } = 0;

        /// <summary>
        /// type của đối tượng liên kết.
        /// </summary>
        public Type ModelType { get; set; } = null;

        /// <summary>
        /// tên của field hiển thị, nếu field đó cùng nằm trong dto.
        /// </summary>
        public string ModelDisplayField { get; set; } = null;

        /// <summary>
        /// mã nhóm.
        /// </summary>
        public string GroupCode { get; set; } = null;

        /// <summary>
        /// tên nhóm.
        /// </summary>
        public string GroupName { get; set; } = null;

        /// <summary>
        /// số col chiếm ở grid.
        /// </summary>
        public int GridCol { get; set; } = 6;

        /// <summary>
        /// ẩn ở màn hình xem chi tiết.
        /// </summary>
        public bool HideOnDetail { get; set; } = false;

        /// <summary>
        /// là key field (dùng trong tìm kiếm).
        /// </summary>
        public bool IsModelKey { get; set; } = false;

        /// <summary>
        /// là field hiển thị (dùng trong tìm kiếm).
        /// </summary>
        public bool IsModelName { get; set; } = false;

        /// <summary>
        /// cho phép sort theo field này?.
        /// </summary>
        public bool Sortable { get; set; } = false;

        /// <summary>
        /// script xử lý hiển thị.
        /// </summary>
        public string ScriptDisplay { get; set; } = null;
    }
}
