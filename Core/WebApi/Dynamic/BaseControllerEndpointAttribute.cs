namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Base controller endpoint.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class BaseControllerEndpointAttribute : Attribute,
        IControllerEndpointAttribute
    {
        /// <inheritdoc/>
        public abstract Type EndpointType { get; }

        /// <inheritdoc/>
        public string[] RequiredAnyScopes { get; set; }

        /// <inheritdoc/>
        public string[] RequiredAnyIssuers { get; set; }
    }
}
