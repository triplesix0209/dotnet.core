using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace TripleSix.Core.Helpers
{
    public static class HttpHelper
    {
        public static string GetValue(this IHeaderDictionary header, string key)
        {
            key = key.Trim().ToCamelCase();
            return header.ContainsKey(key) ? header[key].First() : null;
        }

        public static TResult GetValue<TResult>(
            this IHeaderDictionary header,
            string key,
            Func<string, TResult> converter = null,
            TResult defaultValue = default(TResult))
        {
            var value = GetValue(header, key);
            if (converter is not null) return converter(value);

            var typeCode = Type.GetTypeCode(typeof(TResult));
            if (typeCode == TypeCode.Empty) throw new InvalidCastException();

            return (TResult)Convert.ChangeType(value, typeCode);
        }
    }
}
