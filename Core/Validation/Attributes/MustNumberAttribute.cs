namespace TripleSix.Core.Validation
{
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
