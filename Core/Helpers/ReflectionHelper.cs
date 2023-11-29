using System.ComponentModel;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace TripleSix.Core.Helpers
{
    /// <summary>
    /// Helper xử lý reflection.
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Lấy danh sách type chỉ định và các type mà nó kế thừa.
        /// </summary>
        /// <param name="type">Type chỉ định.</param>
        /// <returns>Danh sách type.</returns>
        public static IEnumerable<Type> BaseTypesAndSelf(this Type? type)
        {
            while (type != null)
            {
                yield return type;

                if (type.BaseType == null) break;
                type = type.BaseType;
            }
        }

        /// <summary>
        /// Kiểm tra một type có thể null hay không.
        /// </summary>
        /// <param name="type">Type cần kiểm tra.</param>
        /// <returns><c>True</c> nếu type có thể null, ngược lại thì <c>False</c>.</returns>
        public static bool IsNullableType(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null || type.IsClass;
        }

        /// <summary>
        /// Kiểm tra type có phải là con của 1 generic type hay không.
        /// </summary>
        /// <param name="type">Type cần kiểm tra.</param>
        /// <param name="genericType">Generic type làm đối chiếu.</param>
        /// <returns><c>True</c> nếu type là con của generic type chỉ định, ngược lại là <c>False</c>.</returns>
        public static bool IsSubclassOfOpenGeneric(this Type type, Type genericType)
        {
            if (!genericType.IsGenericType)
                return false;

            while (type != null && type != typeof(object))
            {
                var cur = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
                if (genericType == cur)
                    return true;

                if (type.BaseType == null) break;
                type = type.BaseType;
            }

            return false;
        }

        /// <summary>
        /// Kiểm tra type có implement 1 generic type hay không.
        /// </summary>
        /// <param name="type">Type cần kiểm tra.</param>
        /// <param name="genericType">Generic type làm đối chiếu.</param>
        /// <returns><c>True</c> nếu type có implement generic type chỉ định, ngược lại là <c>False</c>.</returns>
        public static bool IsAssignableToGenericType(this Type type, Type genericType)
        {
            var interfaceTypes = type.GetInterfaces();
            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
                return true;

            var baseType = type.BaseType;
            if (baseType == null) return false;

            return IsAssignableToGenericType(baseType, genericType);
        }

        /// <summary>
        /// Lấy danh sách tham số của 1 generic chỉnh định.
        /// </summary>
        /// <param name="type">Type cần kiểm tra.</param>
        /// <param name="genericType">Generic type làm đối chiếu.</param>
        /// <returns>Danh sách tham số.</returns>
        public static Type[] GetGenericArguments(this Type type, Type genericType)
        {
            var interfaceTypes = type.GetInterfaces();
            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return it.GetGenericArguments();
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
                return type.GetGenericArguments();

            var baseType = type.BaseType;
            if (baseType == null) return Array.Empty<Type>();

            return GetGenericArguments(baseType, genericType);
        }

        /// <summary>
        /// Lấy chính xác type, nếu type có thể null thì trả về type gốc.
        /// </summary>
        /// <param name="type">Type cần xử lý.</param>
        /// <returns>Type dữ liệu không null.</returns>
        public static Type GetUnderlyingType(this Type type)
        {
            var result = Nullable.GetUnderlyingType(type);
            if (result == null) return type;
            return result;
        }

        /// <summary>
        /// Lấy các property public của type.
        /// </summary>
        /// <param name="type">Type cần xử lý.</param>
        /// <returns>Danh sách property.</returns>
        public static PropertyInfo[] GetPublicProperties(this Type type)
        {
            if (type.IsInterface)
            {
                var propertyInfos = new List<PropertyInfo>();

                var considered = new List<Type>();
                var queue = new Queue<Type>();
                considered.Add(type);
                queue.Enqueue(type);
                while (queue.Count > 0)
                {
                    var subType = queue.Dequeue();
                    foreach (var subInterface in subType.GetInterfaces())
                    {
                        if (considered.Contains(subInterface)) continue;

                        considered.Add(subInterface);
                        queue.Enqueue(subInterface);
                    }

                    var typeProperties = subType.GetProperties(
                        BindingFlags.FlattenHierarchy
                        | BindingFlags.Public
                        | BindingFlags.Instance);

                    var newPropertyInfos = typeProperties
                        .Where(x => !propertyInfos.Contains(x));

                    propertyInfos.InsertRange(0, newPropertyInfos);
                }

                return propertyInfos.ToArray();
            }

            return type.GetProperties(BindingFlags.FlattenHierarchy
                | BindingFlags.Public | BindingFlags.Instance);
        }

        /// <summary>
        /// Lấy tên hiển thị của type.
        /// </summary>
        /// <param name="type">Type cần xử lý.</param>
        /// <returns>Tên hiển thị của property.</returns>
        public static string GetDisplayName(this Type type)
        {
            var displayNameAttr = type.GetCustomAttribute<DisplayNameAttribute>();
            if (displayNameAttr != null && displayNameAttr.DisplayName.IsNotNullOrEmpty())
                return displayNameAttr.DisplayName.Trim().ToTitleCase();

            var commentAttr = type.GetCustomAttribute<CommentAttribute>();
            if (commentAttr != null && commentAttr.Comment.IsNotNullOrEmpty())
                return commentAttr.Comment.Trim().ToTitleCase();

            return type.Name;
        }

        /// <summary>
        /// Lấy tên hiển thị của property.
        /// </summary>
        /// <param name="propertyInfo">Property cần xử lý.</param>
        /// <returns>Tên hiển thị của property.</returns>
        public static string GetDisplayName(this MemberInfo propertyInfo)
        {
            var displayNameAttr = propertyInfo.GetCustomAttribute<DisplayNameAttribute>();
            if (displayNameAttr != null && displayNameAttr.DisplayName.IsNotNullOrEmpty())
                return displayNameAttr.DisplayName.Trim().ToTitleCase();

            return propertyInfo.Name;
        }
    }
}