namespace TripleSix.Core.Mappers
{
    /// <summary>
    /// Loại bỏ property khi map.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class IgnorePropertyAttribute : Attribute
    {
        /// <summary>
        /// Loại bỏ property khi map.
        /// </summary>
        /// <param name="propertyName">Tên property lỗi bỏ khi map.</param>
        public IgnorePropertyAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        /// <summary>
        /// Tên property lỗi bỏ khi map.
        /// </summary>
        public string PropertyName { get; }
    }
}
