using System.Collections.Generic;
using System.Linq;

namespace TripleSix.CoreOld.Helpers
{
    public static class ArrayListHelper
    {
        public static bool ContainAny<T>(this T[] array, params T[] values)
        {
            foreach (var value in values)
            {
                if (array.Contains(value))
                    return true;
            }

            return false;
        }

        public static bool ContainAny<T>(this IList<T> list, params T[] values)
        {
            foreach (var value in values)
            {
                if (list.Contains(value))
                    return true;
            }

            return false;
        }

        public static bool ContainAny<T>(this IEnumerable<T> enumerable, params T[] values)
        {
            foreach (var value in values)
            {
                if (enumerable.Contains(value))
                    return true;
            }

            return false;
        }

        public static bool IsNullOrEmpty<T>(this T[] array)
        {
            return array == null || array.Length == 0;
        }

        public static bool IsNullOrEmpty<T>(this IList<T> list)
        {
            return list == null || list.Count == 0;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || enumerable.Count() == 0;
        }

        public static bool IsNotNullOrEmpty<T>(this T[] array)
        {
            return array != null && array.Length > 0;
        }

        public static bool IsNotNullOrEmpty<T>(this IList<T> list)
        {
            return list != null && list.Count > 0;
        }

        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable != null && enumerable.Count() > 0;
        }

        public static string JoinString(this string[] array, string separator)
        {
            if (array.Count(x => x.IsNotNullOrWhiteSpace()) == 0) return null;
            return string.Join(separator, array.Where(x => x.IsNotNullOrWhiteSpace()));
        }

        public static string JoinString(this IList<string> list, string separator)
        {
            if (list.Count(x => x.IsNotNullOrWhiteSpace()) == 0) return null;
            return string.Join(separator, list.Where(x => x.IsNotNullOrWhiteSpace()));
        }

        public static string JoinString(this IEnumerable<string> enumerable, string separator)
        {
            if (enumerable.Count(x => x.IsNotNullOrWhiteSpace()) == 0) return null;
            return string.Join(separator, enumerable.Where(x => x.IsNotNullOrWhiteSpace()));
        }
    }
}
