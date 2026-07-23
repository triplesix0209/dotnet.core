namespace TripleSix.Core.Attributes
{
    /// <summary>
    /// Thiết lập cho Setting.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class SettingAttribute : Attribute
    {
        /// <summary>
        /// Quy định mã code cho đối tượng.
        /// </summary>
        public SettingAttribute()
        {
        }

        /// <summary>
        /// Giá trị mặc định.
        /// </summary>
        public string? DefaultValue { get; set; }

        /// <summary>
        /// Mô tả.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Công khai hay không ?.
        /// </summary>
        public bool IsPublic { get; set; } = false;

        /// <summary>
        /// Cho phép cập nhật hay không ?.
        /// </summary>
        public bool CanUpdate { get; set; } = false;
    }
}
