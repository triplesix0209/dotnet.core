using System;
using TripleSix.AutoAdmin.Enums;

namespace TripleSix.AutoAdmin.Attributes
{
    /// <summary>
    /// cấu hình auto admin method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AdminMethodAttribute : Attribute
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
        /// bật/tắt controller.
        /// </summary>
        public bool Enable { get; set; } = true;

        /// <summary>
        /// loại phương thức
        /// </summary>
        public AdminMethodTypes Type { get; set; } = 0;
    }
}
