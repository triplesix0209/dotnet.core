using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Jsons
{
    /// <summary>
    /// Base contract resolver.
    /// </summary>
    public class BaseContractResolver : CamelCasePropertyNamesContractResolver
    {
        /// <inheritdoc/>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return base.CreateProperties(type, memberSerialization)
                .OrderBy(p => p.DeclaringType.BaseTypesAndSelf().Count())
                .ToList();
        }
    }
}
