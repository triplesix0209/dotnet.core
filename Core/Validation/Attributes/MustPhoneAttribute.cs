namespace TripleSix.Core.Validation
{
    /// <summary>
    /// Kiểm tra giá trị của property phải là số điện thoại.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MustPhoneAttribute : Attribute
    {
        /// <summary>
        /// Kiểm tra giá trị của property phải là số điện thoại.
        /// </summary>
        public MustPhoneAttribute()
        {
        }
    }
}
