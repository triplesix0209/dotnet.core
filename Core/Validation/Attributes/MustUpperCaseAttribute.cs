namespace TripleSix.Core.Validation
{
    /// <summary>
    /// Kiểm tra giá trị của property phải toàn ký tự hoa.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MustUpperCaseAttribute : Attribute
    {
        /// <summary>
        /// Kiểm tra giá trị của property phải toàn ký tự hoa.
        /// </summary>
        public MustUpperCaseAttribute()
        {
        }
    }
}
