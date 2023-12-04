using System;

namespace TripleSix.Core.AutoAdmin
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AdminMethodAttribute : Attribute
    {
        /// <summary>
        /// Admin Dto sử dụng.
        /// </summary>
        public Type AdminType { get; set; }

        /// <summary>
        /// bật/tắt.
        /// </summary>
        public bool Enable { get; set; } = true;

        /// <summary>
        /// loại phương thức.
        /// </summary>
        public AdminMethodTypes Type { get; set; } = 0;

        /// <summary>
        /// tên phương thức.
        /// </summary>
        public string Name { get; set; } = null;
    }
}
