namespace TripleSix.Core.Validation
{
    /// <summary>
    /// Kiểm tra giá trị của property không lớn hơn số chỉ định.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MaxValueAttribute : Attribute
    {
        /// <summary>
        /// Kiểm tra giá trị của property không lớn hơn số chỉ định.
        /// </summary>
        /// <param name="value">Giá trị chỉ định mà property phải nhỏ hơn hoặc bằng.</param>
        public MaxValueAttribute(long value)
        {
            Value = value;
        }

        /// <summary>
        /// Giá trị chỉ định mà property phải nhỏ hơn hoặc bằng.
        /// </summary>
        public long Value { get; }
    }
}
