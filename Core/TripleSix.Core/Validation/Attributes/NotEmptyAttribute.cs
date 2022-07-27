namespace TripleSix.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NotEmptyAttribute : Attribute
    {
        /// <summary>
        /// Kiểm tra giá trị của property không được phép null, rỗng, không có giá trị.
        /// </summary>
        public NotEmptyAttribute()
        {
        }
    }
}
