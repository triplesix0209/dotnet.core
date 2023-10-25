namespace TripleSix.Core.Mappers
{
    /// <summary>
    /// Loại bỏ property không có khi map.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class IgnoreUnmapPropertyAttribute : Attribute
    {
        /// <summary>
        /// Loại bỏ property không có khi map.
        /// </summary>
        public IgnoreUnmapPropertyAttribute()
        {
        }
    }
}
