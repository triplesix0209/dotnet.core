namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Ẩn khỏi tài liệu swagger.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SwaggerHideAttribute : Attribute
    {
    }
}
