using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CpTech.Core.Extensions
{
    public static class ReflectionExtension
    {
        public static bool IsSubclassOfRawGeneric(this Type type, Type genericType)
        {
            while (type != null && type != typeof(object))
            {
                var cur = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
                if (genericType == cur)
                {
                    return true;
                }

                type = type.BaseType;
            }

            return false;
        }

        public static bool IsSameProperties(this Type sourceType, Type targetType)
        {
            if (sourceType == targetType)
            {
                return true;
            }

            var sourceProperties = sourceType.GetProperties();
            var targetProperties = targetType.GetProperties();
            return targetProperties.Length == sourceProperties.Length && sourceProperties.All(sourceProperty
                => targetProperties.All(targetProperty => targetProperty.Name == sourceProperty.Name));
        }

        public static string GetDisplayName(this Type type)
        {
            var displayName =
                (DisplayNameAttribute)type.GetCustomAttributes(typeof(DisplayNameAttribute), true).FirstOrDefault();
            return displayName != null ? displayName.DisplayName : type.Name;
        }

        public static IEnumerable<Type> BaseTypesAndSelf(this Type type)
        {
            while (type != null)
            {
                yield return type;
                type = type.BaseType;
            }
        }
    }
}