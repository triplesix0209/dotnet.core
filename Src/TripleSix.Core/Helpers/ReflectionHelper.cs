using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace TripleSix.Core.Helpers
{
    public static class ReflectionHelper
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

        public static Type GetUnderlyingType(this Type type)
        {
            return Nullable.GetUnderlyingType(type) is null ? type : Nullable.GetUnderlyingType(type);
        }

        public static bool IsNullableType(this Type type)
        {
            return Nullable.GetUnderlyingType(type) is not null || type.IsClass;
        }

        public static object CreateDefaultInstance(this Type type)
        {
            if (type.IsArray)
            {
                var elementType = type.GetElementType();
                var array = Array.CreateInstance(elementType, 1);
                array.SetValue(elementType.CreateDefaultInstance(), 0);
                return array;
            }

            if (type.IsGenericType && typeof(IList).IsAssignableFrom(type))
            {
                var elementType = type.GetGenericArguments()[0];
                var list = Activator.CreateInstance(type) as IList;
                list.Add(elementType.CreateDefaultInstance());
                return list;
            }

            return Activator.CreateInstance(type);
        }
    }
}