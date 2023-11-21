namespace TripleSix.Core.Validation
{
    /// <summary>
    /// Kiểm tra giá trị của property chỉ được phép chứa chữ số.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MustNumberAttribute : Attribute
    {
        /// <summary>
        /// Kiểm tra giá trị của property chỉ được phép chứa chữ số.
        /// </summary>
        public MustNumberAttribute()
        {
        }
    }
}
