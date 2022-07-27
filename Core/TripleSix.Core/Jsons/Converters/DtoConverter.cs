using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using TripleSix.Core.Helpers;
using TripleSix.Core.Types;

namespace TripleSix.Core.Jsons
{
    public class DtoConverter : JsonConverter<IDto>
    {
        private readonly JsonSerializer _jsonSerializer;

        public DtoConverter()
        {
            _jsonSerializer = JsonSerializer.Create(JsonHelper.SerializerSettings);
            _jsonSerializer.ContractResolver = new BaseContractResolver();
        }

        public DtoConverter(IContractResolver contractResolver)
        {
            _jsonSerializer = JsonSerializer.Create(JsonHelper.SerializerSettings);
            _jsonSerializer.ContractResolver = contractResolver;
        }

        public override IDto? ReadJson(JsonReader reader, Type objectType, IDto? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var data = serializer.Deserialize(reader) as JObject;
            if (data == null) return null;

            var result = data.ToObject(objectType, _jsonSerializer) as IDto;
            if (result == null) return null;

            foreach (var p in objectType.GetProperties())
            {
                if (!p.CanRead)
                    continue;

                if (!data.Properties().Any(x => x.Name.ToLower() == p.Name.ToLower()))
                    continue;

                (result as IDto)?.SetPropertyChanged(p.Name, true);
            }

            return result;
        }

        public override void WriteJson(JsonWriter writer, IDto? value, JsonSerializer serializer)
        {
            _jsonSerializer.Serialize(writer, value);
        }
    }
}
