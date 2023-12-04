using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using TripleSix.Core.Attributes;

namespace TripleSix.Core.Helpers
{
    public static class EnumHelper
    {
        public static string GetDescription<T>(this T? value)
            where T : struct, Enum
        {
            return GetDescription(typeof(T), value);
        }

        public static string GetDescription<T>(this T value)
            where T : struct, Enum
        {
            return GetDescription(typeof(T), value);
        }

        public static string GetDescription(Type enumType, object value)
        {
            if (value == null) return null;

            MemberInfo mi = null;
            if (value is string)
            {
                mi = enumType.GetMember(value.ToString() ?? string.Empty).FirstOrDefault();
            }
            else
            {
                var valueName = GetName(enumType, value);
                if (valueName is not null)
                    mi = enumType.GetMember(valueName).FirstOrDefault();
            }

            var attrDesc = (DescriptionAttribute)mi?
                .GetCustomAttributes(typeof(DescriptionAttribute), true)
                .FirstOrDefault();

            return attrDesc?.Description ?? GetName(enumType, value);
        }

        public static ErrorDataAttribute GetErrorData<T>(this T? value)
            where T : struct, Enum
        {
            return GetErrorData(typeof(T), value);
        }

        public static ErrorDataAttribute GetErrorData<T>(this T value)
            where T : struct, Enum
        {
            return GetErrorData(typeof(T), value);
        }

        public static ErrorDataAttribute GetErrorData(Type enumType, object value)
        {
            if (value == null) return null;

            MemberInfo mi = null;
            if (value is string)
            {
                mi = enumType.GetMember(value.ToString() ?? string.Empty).FirstOrDefault();
            }
            else
            {
                var valueName = GetName(enumType, value);
                if (valueName is not null)
                    mi = enumType.GetMember(valueName).FirstOrDefault();
            }

            return (ErrorDataAttribute)mi?
                .GetCustomAttributes(typeof(ErrorDataAttribute), true)
                .FirstOrDefault();
        }

        public static string GetName<T>(this T? value)
            where T : struct, Enum
        {
            return value == null ? null : Enum.GetName(typeof(T), value.Value);
        }

        public static string GetName<T>(this T value)
            where T : struct, Enum
        {
            return Enum.GetName(typeof(T), value);
        }

        public static string GetName(Type enumType, object value)
        {
            return value == null ? null : Enum.GetName(enumType, value);
        }

        public static string[] GetNames<T>()
            where T : struct, Enum
        {
            return Enum.GetNames(typeof(T));
        }

        public static string[] GetNames(Type enumType)
        {
            return Enum.GetNames(enumType);
        }

        public static IEnumerable<T> GetValues<T>()
            where T : struct, Enum
        {
            return GetValues(typeof(T)).Cast<T>();
        }

        public static Array GetValues(Type enumType)
        {
            var values = Enum.GetValues(enumType).Cast<int>();
            return values.Where(x => x >= 0).OrderBy(x => x)
                .Concat(values.Where(x => x < 0).OrderByDescending(x => x))
                .ToArray();
        }

        public static T Parse<T>(object value, bool ignoreCase = true)
            where T : Enum
        {
            return (T)Parse(typeof(T), value, ignoreCase);
        }

        public static object Parse(Type enumType, object value, bool ignoreCase = true)
        {
            if (value is string str)
                return Enum.Parse(enumType, str, ignoreCase);

            return Enum.ToObject(enumType, Convert.ToInt32(value));
        }

        public static bool TryParse<T>(object value, out object result, bool ignoreCase = true)
            where T : Enum
        {
            return TryParse(typeof(T), value, out result, ignoreCase);
        }

        public static bool TryParse(Type enumType, object value, out object result, bool ignoreCase = true)
        {
            try
            {
                result = Parse(enumType, value, ignoreCase);
                return true;
            }
            catch
            {
                result = Enum.ToObject(enumType, -999999);
                return false;
            }
        }
    }
}
