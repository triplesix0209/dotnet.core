using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace TripleSix.Core.Extensions
{
    public static class HttpExtension
    {
        public static string GetHeaderValue(this IHeaderDictionary header, string key)
        {
            key = key.Trim().ToCamelCase();
            return header.ContainsKey(key) ? header[key].First() : null;
        }

        public static TResult GetHeaderValue<TResult>(this IHeaderDictionary header, string key, Func<string, TResult> converter, TResult defaultValue = default(TResult))
        {
            try
            {
                return converter(GetHeaderValue(header, key));
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}