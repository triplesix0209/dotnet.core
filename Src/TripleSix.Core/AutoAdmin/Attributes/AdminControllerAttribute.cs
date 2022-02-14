using System;

namespace TripleSix.Core.AutoAdmin
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AdminControllerAttribute : Attribute
    {
        /// <summary>
        /// Admin Dto sử dụng của controller.
        /// </summary>
        public Type AdminType { get; set; }

        /// <summary>
        /// Entity sử dụng của controller.
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// tên gọi.
        /// </summary>
        public string Name { get; set; } = null;

        /// <summary>
        /// bật/tắt controller.
        /// </summary>
        public bool Enable { get; set; } = true;

        /// <summary>
        /// ẩn hiển thị ở menu.
        /// </summary>
        public bool HideOnMenu { get; set; } = false;

        /// <summary>
        /// bật/tắt các phương thức xem dữ liệu.
        /// </summary>
        public bool EnableRead { get; set; } = true;

        /// <summary>
        /// bật/tắt các tính năng tạo dữ liệu.
        /// </summary>
        public bool EnableCreate { get; set; } = true;

        /// <summary>
        /// bật/tắt các tính năng sửa dữ liệu.
        /// </summary>
        public bool EnableUpdate { get; set; } = true;

        /// <summary>
        /// bật/tắt các tính năng xóa dữ liệu.
        /// </summary>
        public bool EnableDelete { get; set; } = true;

        /// <summary>
        /// bật/tắt các phương thức xem lịch sử thay đổi.
        /// </summary>
        public bool EnableChangeLog { get; set; } = true;

        /// <summary>
        /// bật/tắt các tính năng xuất dữ liệu.
        /// </summary>
        public bool EnableExport { get; set; } = true;

        /// <summary>
        /// icon mục.
        /// </summary>
        public string Icon { get; set; } = null;

        /// <summary>
        /// mã nhóm.
        /// </summary>
        public string GroupCode { get; set; } = null;

        /// <summary>
        /// tên nhóm.
        /// </summary>
        public string GroupName { get; set; } = null;

        /// <summary>
        /// icon nhóm.
        /// </summary>
        public string GroupIcon { get; set; } = null;

        /// <summary>
        /// thứ tự hiển thị.
        /// </summary>
        public int LoadOrder { get; set; } = 0;

        /// <summary>
        /// mã nhóm quyền.
        /// </summary>
        public string PermissionGroup { get; set; } = null;
    }
}
