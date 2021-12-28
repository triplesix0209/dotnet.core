using System.Collections.Generic;

namespace CpTech.Core.Extensions
{
    public static class ArrayListExtension
    {
        public static bool IsNullOrEmpty<T>(this T[] array)
        {
            return array == null || array.Length == 0;
        }

        public static bool IsNotNullOrEmpty<T>(this T[] array)
        {
            return array != null && array.Length > 0;
        }

        public static bool IsNullOrEmpty<T>(this IList<T> list)
        {
            return list == null || list.Count == 0;
        }

        public static bool IsNotNullOrEmpty<T>(this IList<T> list)
        {
            return list != null && list.Count > 0;
        }
    }
}