namespace TripleSix.Core.Validation
{
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
