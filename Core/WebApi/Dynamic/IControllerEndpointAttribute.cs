using System.Reflection;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Controller endpoint attribute.
    /// </summary>
    public interface IControllerEndpointAttribute
    {
        /// <summary>
        /// Chuyển hóa sang TypeInfo.
        /// </summary>
        /// <returns><see cref="TypeInfo"/>.</returns>
        public abstract TypeInfo ToEndpointTypeInfo();
    }
}
