namespace TripleSix.Core.Validation
{
    /// <summary>
    /// Kiểm tra giá trị của property phải đúng regular expression chỉ định.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MustRegExrAttribute : Attribute
    {
        /// <summary>
        /// Kiểm tra giá trị của property phải đúng regular expression chỉ định.
        /// </summary>
        /// <param name="patternExr">Regular expression chỉ định.</param>
        /// <param name="patternName">Tên của regular expression.</param>
        public MustRegExrAttribute(string patternExr, string? patternName = null)
        {
            PatternExr = patternExr;
            PatternName = patternName;
        }

        /// <summary>
        /// Regular expression chỉ định.
        /// </summary>
        public string PatternExr { get; }

        /// <summary>
        /// Tên của regular expression.
        /// </summary>
        public string? PatternName { get; }
    }
}
