namespace TripleSix.Core.AutoAdmin
{
    /// <summary>
    /// Cài đặt Admin model.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AdminModelAttribute : Attribute
    {
        /// <summary>
        /// Entity sử dụng.
        /// </summary>
        public Type EntityType { get; set; }
    }
}
