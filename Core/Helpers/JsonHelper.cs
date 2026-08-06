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
        public static readonly JsonConverter[] Converters =
        [
            new TimestampConverter(),
        ];

        /// <summary>
        /// Cấu hình Json Serializer mặc định.
        /// </summary>
        public static readonly JsonSerializerOptions SerializerOptions = CreateDefaultOptions();

        private static readonly JsonNodeOptions _defaultNodeOptions = new() { PropertyNameCaseInsensitive = true };

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
                return GetInheritanceDepth(memberInfo?.DeclaringType);
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
        public static string ToJsonText(this object obj)
        {
            return JsonSerializer.Serialize(obj, SerializerOptions);
        }

        /// <summary>
        /// Mã hóa đối tượng thành chuỗi JSON.
        /// </summary>
        /// <param name="obj">Đối tượng sẽ được mã hóa.</param>
        /// <param name="ignorePropertyNames">Danh sách property loại bỏ.</param>
        /// <returns>Chuỗi JSON ứng với đối tượng chỉ định.</returns>
        public static string ToJsonText(this object obj, params string[] ignorePropertyNames)
        {
            if (ignorePropertyNames == null || ignorePropertyNames.Length == 0)
                return obj.ToJsonText();

            var ignoreProps = new HashSet<string>(ignorePropertyNames, StringComparer.OrdinalIgnoreCase);
            var resolver = new DefaultJsonTypeInfoResolver();
            resolver.Modifiers.Add(BaseContractResolverModifier);
            resolver.Modifiers.Add(typeInfo =>
            {
                if (typeInfo.Kind != JsonTypeInfoKind.Object) return;
                foreach (var prop in typeInfo.Properties)
                {
                    if (prop.Name.IsNotNullOrEmpty() && ignoreProps.Contains(prop.Name))
                        prop.ShouldSerialize = (_, _) => false;
                }
            });

            var options = new JsonSerializerOptions(SerializerOptions)
            {
                TypeInfoResolver = resolver,
            };
            return JsonSerializer.Serialize(obj, options);
        }

        /// <summary>
        /// Chuyển đổi chuỗi JSON thành JsonNode.
        /// </summary>
        /// <param name="json">Chuỗi JSON cần đọc.</param>
        /// <param name="nodeOptions">Cấu hình JsonNode.</param>
        /// <param name="documentOptions">Cấu hình JsonDocument.</param>
        /// <returns><see cref="JsonNode"/>.</returns>
        public static JsonNode? ToJsonNode(this string json, JsonNodeOptions? nodeOptions = null, JsonDocumentOptions documentOptions = default)
        {
            if (json.IsNullOrEmpty()) return null;
            nodeOptions ??= _defaultNodeOptions;
            return JsonNode.Parse(json, nodeOptions, documentOptions);
        }

        /// <summary>
        /// Chuyển đổi object thành JsonNode.
        /// </summary>
        /// <param name="obj">Object cần đọc.</param>
        /// <returns><see cref="JsonNode"/>.</returns>
        public static JsonNode? ToJsonNode(this object obj)
        {
            if (obj == null) return null;
            return JsonSerializer.SerializeToNode(obj, SerializerOptions);
        }

        /// <summary>
        /// Chuyển đổi object thành JsonElement.
        /// </summary>
        /// <param name="obj">Object cần đọc.</param>
        /// <returns><see cref="JsonElement"/>.</returns>
        public static JsonElement? ToJsonElement(this object obj)
        {
            if (obj == null) return null;
            return JsonSerializer.SerializeToElement(obj, SerializerOptions);
        }

        /// <summary>
        /// Chuyển đổi chuỗi JSON thành đối tượng.
        /// </summary>
        /// <param name="json">Chuỗi Json cần đọc.</param>
        /// <param name="type">Loại đối tượng.</param>
        /// <returns>Đối tượng được chuyển đổi từ chuỗi JSON.</returns>
        public static object? ToObject(this string json, Type type)
        {
            if (json.IsNullOrEmpty()) return null;
            return JsonSerializer.Deserialize(json, type, SerializerOptions);
        }

        /// <summary>
        /// Chuyển đổi chuỗi JSON thành đối tượng.
        /// </summary>
        /// <typeparam name="T">Loại đối tượng.</typeparam>
        /// <param name="json">Chuỗi Json cần đọc.</param>
        /// <returns>Đối tượng được chuyển đổi từ chuỗi JSON.</returns>
        public static T? ToObject<T>(this string json)
        {
            if (json.IsNullOrEmpty()) return default;
            return JsonSerializer.Deserialize<T>(json, SerializerOptions);
        }

        /// <summary>
        /// Chuyển đổi JsonNode thành đối tượng.
        /// </summary>
        /// <typeparam name="T">Loại đối tượng.</typeparam>
        /// <param name="node"><see cref="JsonNode"/>.</param>
        /// <returns>Đối tượng được chuyển đổi.</returns>
        public static T? ToObject<T>(this JsonNode node)
        {
            if (node == null) return default;
            return node.Deserialize<T>(SerializerOptions);
        }

        /// <summary>
        /// Chuyển đổi JsonNode thành đối tượng.
        /// </summary>
        /// <param name="node"><see cref="JsonNode"/>.</param>
        /// <param name="type">Loại đối tượng.</param>
        /// <returns>Đối tượng được chuyển đổi.</returns>
        public static object? ToObject(this JsonNode node, Type type)
        {
            if (node == null) return null;
            return node.Deserialize(type, SerializerOptions);
        }

        private static JsonSerializerOptions CreateDefaultOptions()
        {
            var resolver = new DefaultJsonTypeInfoResolver();
            resolver.Modifiers.Add(BaseContractResolverModifier);
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
                TypeInfoResolver = resolver,
            };
            foreach (var converter in Converters)
                options.Converters.Add(converter);
            return options;
        }

        private static int GetInheritanceDepth(Type? type)
        {
            var depth = 0;
            while (type != null)
            {
                depth++;
                type = type.BaseType;
            }

            return depth;
        }
    }
}
