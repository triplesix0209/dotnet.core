using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using TripleSix.Core.Jsons;

namespace TripleSix.Core.Helpers
{
    /// <summary>
    /// Helper xử lý json.
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// Danh sách Json Converter mặc định.
        /// </summary>
        public static readonly JsonConverter[] Converters = new JsonConverter[]
        {
            new TimestampConverter(),
        };

        /// <summary>
        /// Cấu hình Json Serializer mặc định.
        /// </summary>
        public static readonly JsonSerializerOptions SerializerOptions = CreateDefaultOptions();

        /// <summary>
        /// Modifier cho DefaultJsonTypeInfoResolver sắp xếp properties theo thứ tự kế thừa.
        /// </summary>
        /// <param name="typeInfo"><see cref="JsonTypeInfo"/>.</param>
        public static void BaseContractResolverModifier(JsonTypeInfo typeInfo)
        {
            if (typeInfo.Kind != JsonTypeInfoKind.Object) return;

            var properties = typeInfo.Properties.ToList();
            typeInfo.Properties.Clear();
            foreach (var prop in properties.OrderBy(p =>
            {
                var memberInfo = p.AttributeProvider as MemberInfo;
                return memberInfo?.DeclaringType?.BaseTypesAndSelf().Count() ?? 0;
            }))
            {
                typeInfo.Properties.Add(prop);
            }
        }

        /// <summary>
        /// Mã hóa đối tượng thành chuỗi JSON.
        /// </summary>
        /// <param name="obj">Đối tượng sẽ được mã hóa.</param>
        /// <returns>Chuỗi JSON ứng với đối tượng chỉ định.</returns>
        public static string ToJson(this object obj)
        {
            return JsonSerializer.Serialize(obj, SerializerOptions);
        }

        /// <summary>
        /// Mã hóa đối tượng thành chuỗi JSON.
        /// </summary>
        /// <param name="obj">Đối tượng sẽ được mã hóa.</param>
        /// <param name="ignorePropertyNames">Danh sách property loại bỏ.</param>
        /// <returns>Chuỗi JSON ứng với đối tượng chỉ định.</returns>
        public static string ToJson(this object obj, params string[] ignorePropertyNames)
        {
            var ignoreProps = new HashSet<string>(ignorePropertyNames.Select(x => x.ToLower()));
            var resolver = new DefaultJsonTypeInfoResolver();
            resolver.Modifiers.Add(BaseContractResolverModifier);
            resolver.Modifiers.Add(typeInfo =>
            {
                if (typeInfo.Kind != JsonTypeInfoKind.Object) return;
                foreach (var prop in typeInfo.Properties)
                {
                    if (prop.Name.IsNotNullOrEmpty() && ignoreProps.Contains(prop.Name.ToLower()))
                    {
                        prop.ShouldSerialize = (_, _) => false;
                    }
                }
            });

            var options = new JsonSerializerOptions(SerializerOptions)
            {
                TypeInfoResolver = resolver
            };
            return JsonSerializer.Serialize(obj, options);
        }

        /// <summary>
        /// Chuyển đổi chuổi JSON thành JsonNode.
        /// </summary>
        /// <param name="json">Chuổi JSON cần đọc.</param>
        /// <returns><see cref="JsonNode"/>.</returns>
        public static JsonNode? ToJsonNode(this string json)
        {
            if (json.IsNullOrEmpty()) return null;
            return JsonNode.Parse(json);
        }

        /// <summary>
        /// Chuyển đổi chuổi JSON thành đối tượng.
        /// </summary>
        /// <param name="json">Chuổi Json cần đọc.</param>
        /// <param name="type">Loại đối tượng.</param>
        /// <returns>Đối tượng được chuyển đổi từ chuỗi JSON.</returns>
        public static object? ToObject(this string json, Type type)
        {
            if (json.IsNullOrEmpty()) return null;
            return JsonSerializer.Deserialize(json, type, SerializerOptions);
        }

        /// <summary>
        /// Chuyển đổi chuổi JSON thành đối tượng.
        /// </summary>
        /// <typeparam name="T">Loại đối tượng.</typeparam>
        /// <param name="json">Chuổi Json cần đọc.</param>
        /// <returns>Đối tượng được chuyển đổi từ chuỗi JSON.</returns>
        public static T? ToObject<T>(this string json)
        {
            if (json.IsNullOrEmpty()) return default;
            return JsonSerializer.Deserialize<T>(json, SerializerOptions);
        }

        private static JsonSerializerOptions CreateDefaultOptions()
        {
            var resolver = new DefaultJsonTypeInfoResolver();
            resolver.Modifiers.Add(BaseContractResolverModifier);
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                TypeInfoResolver = resolver,
            };
            foreach (var converter in Converters)
                options.Converters.Add(converter);
            return options;
        }
    }
}
