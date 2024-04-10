namespace TripleSix.Core.Attributes
{
    /// <summary>
    /// Mã code.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class CodeValueAttribute : Attribute
    {
        /// <summary>
        /// Quy định mã code cho đối tượng.
        /// </summary>
        /// <param name="value">Giá trị mã code.</param>
        public CodeValueAttribute(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Giá trị mã code.
        /// </summary>
        public string Value { get; }
    }
}
