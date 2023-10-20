namespace TripleSix.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MustLowerCaseAttribute : Attribute
    {
        /// <summary>
        /// Kiểm tra giá trị của property phải toàn ký tự thường.
        /// </summary>
        public MustLowerCaseAttribute()
        {
        }
    }
}
