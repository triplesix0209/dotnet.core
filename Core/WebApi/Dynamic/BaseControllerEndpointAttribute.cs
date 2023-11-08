﻿using System.Reflection;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Base controller endpoint.
    /// </summary>
    /// <typeparam name="TController">Controller type.</typeparam>
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class BaseControllerEndpointAttribute<TController> : Attribute,
        IControllerEndpointAttribute
        where TController : BaseController
    {
        /// <summary>
        /// Chuyển hóa sang TypeInfo.
        /// </summary>
        /// <returns><see cref="TypeInfo"/>.</returns>
        public abstract TypeInfo ToEndpointTypeInfo();
    }
}
