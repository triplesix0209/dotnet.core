using CpTech.Core.JsonSerializers.ContractResolvers;
using CpTech.Core.JsonSerializers.Converters;
using Newtonsoft.Json;

namespace CpTech.Core.JsonSerializers
{
    public static class JsonHelper
    {
        public static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new BaseContractResolver(),
            Converters = new JsonConverter[]
            {
                new TimestampConverter(),
                new PhoneConverter(),
            },
        };

        public static string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value, SerializerSettings);
        }

        public static string SerializeObject(object value, BaseContractResolver contractResolver)
        {
            var settings = SerializerSettings;
            settings.ContractResolver = contractResolver;
            return JsonConvert.SerializeObject(value, settings);
        }

        public static string SerializeObject(object value, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(value, settings);
        }
    }
}