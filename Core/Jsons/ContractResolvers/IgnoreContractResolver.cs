using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Jsons
{
    /// <summary>
    /// Ignore contract resolver.
    /// </summary>
    public class IgnoreContractResolver : BaseContractResolver
    {
        private readonly HashSet<string> _ignoreProps;

        /// <summary>
        /// Ignore contract resolver.
        /// </summary>
        /// <param name="propertyNamesToIgnore">List property name to ignore.</param>
        public IgnoreContractResolver(IEnumerable<string> propertyNamesToIgnore)
        {
            _ignoreProps = new HashSet<string>(propertyNamesToIgnore.Select(x => x.ToLower()));
        }

        /// <inheritdoc/>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (property.PropertyName.IsNotNullOrEmpty()
                && _ignoreProps.Contains(property.PropertyName.ToLower()))
                property.ShouldSerialize = _ => false;

            return property;
        }
    }
}
