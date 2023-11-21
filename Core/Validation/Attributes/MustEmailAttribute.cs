namespace TripleSix.Core.Validation
{
    /// <summary>
    /// Kiểm tra giá trị của property phải là e-mail.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MustEmailAttribute : Attribute
    {
        /// <summary>
        /// Kiểm tra giá trị của property phải là e-mail.
        /// </summary>
        public MustEmailAttribute()
        {
        }
    }
}
