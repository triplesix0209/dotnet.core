namespace TripleSix.Core.AutoAdmin
{
    /// <summary>
    /// Cài đặt thông tin Admin cho method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AdminMethodAttribute : Attribute
    {
        /// <summary>
        /// Admin model sử dụng.
        /// </summary>
        public Type ModelType { get; set; }

        /// <summary>
        /// Loại phương thức.
        /// </summary>
        public AdminMethodTypes Type { get; set; } = 0;

        /// <summary>
        /// Bật/Tắt.
        /// </summary>
        public bool Enable { get; set; } = true;

        /// <summary>
        /// Ẩn/Hiện.
        /// </summary>
        public bool Render { get; set; } = true;

        /// <summary>
        /// Tên gọi.
        /// </summary>
        public string? Name { get; set; } = null;

        /// <summary>
        /// Icon nhóm (mdi-icon).
        /// </summary>
        public string? Icon { get; set; } = null;
    }
}
