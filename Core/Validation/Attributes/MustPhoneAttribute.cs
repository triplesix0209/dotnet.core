namespace TripleSix.Core.Validation
{
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
