using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Jsons
{
    public class IgnoreContractResolver : BaseContractResolver
    {
        private readonly HashSet<string> _ignoreProps;

        public IgnoreContractResolver(IEnumerable<string> propertyNamesToIgnore)
        {
            _ignoreProps = new HashSet<string>(propertyNamesToIgnore.Select(x => x.ToLower()));
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (!property.PropertyName.IsNullOrEmpty()
                && _ignoreProps.Contains(property.PropertyName.ToLower()))
                property.ShouldSerialize = _ => false;

            return property;
        }
    }
}
