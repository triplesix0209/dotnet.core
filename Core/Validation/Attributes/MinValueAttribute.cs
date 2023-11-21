namespace TripleSix.Core.Validation
{
    /// <summary>
    /// Kiểm tra giá trị của property không nhỏ hơn số chỉ định.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MinValueAttribute : Attribute
    {
        /// <summary>
        /// Kiểm tra giá trị của property không nhỏ hơn số chỉ định.
        /// </summary>
        /// <param name="value">Giá trị chỉ định mà property phải lớn hơn hoặc bằng.</param>
        public MinValueAttribute(long value)
        {
            Value = value;
        }

        /// <summary>
        /// Giá trị chỉ định mà property phải lớn hơn hoặc bằng.
        /// </summary>
        public long Value { get; }
    }
}
