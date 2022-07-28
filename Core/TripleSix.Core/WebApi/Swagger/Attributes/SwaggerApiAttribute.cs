namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Khai báo thông tin API.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class SwaggerApiAttribute : Attribute
    {
        /// <summary>
        /// Khai báo thông tin API.
        /// </summary>
        /// <param name="summary">Mô tả ngắn gọn API.</param>
        public SwaggerApiAttribute(string summary)
        {
            Summary = summary;
            ResponseType = typeof(SuccessResult);
        }

        /// <summary>
        /// Khai báo thông tin API.
        /// </summary>
        /// <param name="responseType">Kiểu dữ liệu trả về.</param>
        public SwaggerApiAttribute(Type responseType)
        {
            ResponseType = responseType;
        }

        /// <summary>
        /// Khai báo thông tin API.
        /// </summary>
        /// <param name="summary">Mô tả ngắn gọn API.</param>
        /// <param name="responseType">Kiểu dữ liệu trả về.</param>
        public SwaggerApiAttribute(string summary, Type responseType)
        {
            Summary = summary;
            ResponseType = responseType;
        }

        /// <summary>
        /// Khai báo thông tin API.
        /// </summary>
        /// <param name="summary">Mô tả ngắn gọn API.</param>
        /// <param name="description">Mô tả chi tiết API.</param>
        /// <param name="responseType">Kiểu dữ liệu trả về.</param>
        public SwaggerApiAttribute(string summary, string description, Type responseType)
        {
            Summary = summary;
            Description = description;
            ResponseType = responseType;
        }

        /// <summary>
        /// Mô tả ngắn gọn API.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Mô tả chi tiết API.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Kiểu dữ liệu trả về.
        /// </summary>
        public Type ResponseType { get; set; }
    }
}
