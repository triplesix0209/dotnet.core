using System.Reflection;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Base controller endpoint.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class BaseControllerEndpointAttribute : Attribute
    {
        protected BaseControllerEndpointAttribute(Type controllerType)
        {
            ControllerType = controllerType;
        }

        /// <summary>
        /// Loại controller.
        /// </summary>
        public Type ControllerType { get; set; }

        /// <summary>
        /// Chuyển hóa sang TypeInfo.
        /// </summary>
        /// <returns><see cref="TypeInfo"/>.</returns>
        public abstract TypeInfo ToEndpointTypeInfo();
    }
}
