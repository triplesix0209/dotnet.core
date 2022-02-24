using System;

namespace TripleSix.Core.AutoAdmin
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AdminFieldAttribute : Attribute
    {
        /// <summary>
        /// có hiển thị field này hay không.
        /// </summary>
        public bool Render { get; set; } = true;

        /// <summary>
        /// loại field.
        /// </summary>
        public AdminFieldTypes Type { get; set; } = 0;

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
        /// cho phép sort theo field này?.
        /// </summary>
        public bool Sortable { get; set; } = true;

        /// <summary>
        /// field được sử dụng để sort.
        /// </summary>
        public string SortByColumn { get; set; } = null;

        /// <summary>
        /// tên field được sử dụng để hiển thị.
        /// </summary>
        public string DisplayBy { get; set; } = null;

        /// <summary>
        /// script xử lý hiển thị.
        /// </summary>
        public string ScriptDisplay { get; set; } = null;

        /// <summary>
        /// type của đối tượng liên kết.
        /// </summary>
        public Type ModelType { get; set; } = null;

        /// <summary>
        /// là key field của model.
        /// </summary>
        public bool IsModelKey { get; set; } = false;

        /// <summary>
        /// là field hiển thị của model.
        /// </summary>
        public bool IsModelText { get; set; } = false;

        /// <summary>
        /// hiển thị trên màn hình xem chi tiết.
        /// </summary>
        public bool RenderOnDetail { get; set; } = true;
    }
}
