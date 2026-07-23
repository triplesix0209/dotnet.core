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
        /// <param name="minLength">Min length allowed.</param>
        /// <param name="maxLength">Max length allowed.</param>
        public MustPhoneAttribute(int minLength = 10, int? maxLength = 12)
        {
            MinLength = minLength;
            MaxLength = maxLength;
        }

        /// <summary>
        /// Độ dài tối thiểu của số điện thoại.
        /// </summary>
        public int MinLength { get; set; } = 10;

        /// <summary>
        /// Độ dài tối đa của số điện thoại.
        /// </summary>
        public int? MaxLength { get; set; } = 12;
    }
}
