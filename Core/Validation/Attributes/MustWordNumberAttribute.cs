namespace TripleSix.Core.Validation
{
    /// <summary>
    /// Kiểm tra giá trị của property chỉ được phép chứa chữ cái hoặc chữ số.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MustWordNumberAttribute : Attribute
    {
        /// <summary>
        /// Kiểm tra giá trị của property chỉ được phép chứa chữ cái hoặc chữ số.
        /// </summary>
        public MustWordNumberAttribute()
        {
        }
    }
}
