using System;
using System.Collections.Generic;
using System.Linq;
using CpTech.Core.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CpTech.Core.JsonSerializers.ContractResolvers
{
    public class BaseContractResolver : CamelCasePropertyNamesContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return base.CreateProperties(type, memberSerialization)
                .OrderBy(p => p.DeclaringType.BaseTypesAndSelf().Count())
                .ToList();
        }
    }
}
