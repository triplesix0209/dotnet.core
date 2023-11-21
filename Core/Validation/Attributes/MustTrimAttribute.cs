namespace TripleSix.Core.Validation
{
    /// <summary>
    /// Kiểm tra giá trị của property không được thừa khoảng trắng trước và sau.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MustTrimAttribute : Attribute
    {
        /// <summary>
        /// Kiểm tra giá trị của property không được thừa khoảng trắng trước và sau.
        /// </summary>
        public MustTrimAttribute()
        {
        }
    }
}
