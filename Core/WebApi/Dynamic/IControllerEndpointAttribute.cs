namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Controller endpoint attribute.
    /// </summary>
    public interface IControllerEndpointAttribute
    {
        /// <summary>
        /// Endpoint type.
        /// </summary>
        Type EndpointType { get; }

        /// <summary>
        /// Danh sách scope cần để truy cập (chỉ cần có 1).
        /// </summary>
        string[] RequiredAnyScopes { get; set; }
    }
}
