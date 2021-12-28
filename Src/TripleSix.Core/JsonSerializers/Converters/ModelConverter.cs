using System;
using System.Linq;
using TripleSix.Core.Dto;
using TripleSix.Core.JsonSerializers.ContractResolvers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TripleSix.Core.JsonSerializers.Converters
{
    public class ModelConverter : JsonConverter<IDto>
    {
        private readonly JsonSerializer _jsonSerializer;

        public ModelConverter()
        {
            _jsonSerializer = JsonSerializer.Create(JsonHelper.SerializerSettings);
            _jsonSerializer.ContractResolver = new BaseContractResolver();
            _jsonSerializer.Converters.Add(new TimestampConverter());
            _jsonSerializer.Converters.Add(new PhoneConverter());
        }

        public ModelConverter(BaseContractResolver contractResolver)
        {
            _jsonSerializer = JsonSerializer.Create(JsonHelper.SerializerSettings);
            _jsonSerializer.ContractResolver = contractResolver;
            _jsonSerializer.Converters.Add(new TimestampConverter());
            _jsonSerializer.Converters.Add(new PhoneConverter());
        }

        public override IDto ReadJson(
            JsonReader reader,
            Type objectType,
            IDto existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            var data = (JObject)serializer.Deserialize(reader);
            if (data == null)
            {
                return null;
            }

            var result = data.ToObject(objectType, _jsonSerializer);
            foreach (var p in objectType.GetProperties())
            {
                if (!p.CanRead)
                {
                    continue;
                }

                if (!data.Properties().Any(x => x.Name.ToLower() == p.Name.ToLower()))
                {
                    continue;
                }

                (result as IPropertyTracking)?.SetPropertyChanged(p.Name, true);
            }

            return (IDto)result;
        }

        public override void WriteJson(JsonWriter writer, IDto value, JsonSerializer serializer)
        {
            _jsonSerializer.Serialize(writer, value);
        }
    }
}