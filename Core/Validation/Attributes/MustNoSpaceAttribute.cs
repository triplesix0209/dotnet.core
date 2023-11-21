namespace TripleSix.Core.Validation
{
    /// <summary>
    /// Kiểm tra giá trị của property không được chứa ký tự khoảng trắng (space).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MustNoSpaceAttribute : Attribute
    {
        /// <summary>
        /// Kiểm tra giá trị của property không được chứa ký tự khoảng trắng (space).
        /// </summary>
        public MustNoSpaceAttribute()
        {
        }
    }
}
