namespace TripleSix.Core.Validation
{
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
