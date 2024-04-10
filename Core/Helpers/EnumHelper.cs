using System.ComponentModel;
using System.Reflection;
using TripleSix.Core.Attributes;

namespace TripleSix.Core.Helpers
{
    /// <summary>
    /// Helper xử lý enum.
    /// </summary>
    public static partial class EnumHelper
    {
        /// <summary>
        /// Lấy mô tả của enum.
        /// </summary>
        /// <typeparam name="T">Loại dữ liệu.</typeparam>
        /// <param name="value">Dữ liệu.</param>
        /// <returns>Chuỗi mô tả giá trị của enum.</returns>
        public static string GetDescription<T>(this T value)
            where T : Enum
        {
            return GetDescription(typeof(T), value) ?? value.ToString();
        }

        /// <summary>
        /// Lấy mã code của enum.
        /// </summary>
        /// <typeparam name="T">Loại dữ liệu.</typeparam>
        /// <param name="value">Dữ liệu.</param>
        /// <returns>Chuỗi mã code của enum.</returns>
        public static string GetCodeValue<T>(this T value)
            where T : Enum
        {
            return GetCodeValue(typeof(T), value) ?? value.ToString();
        }
    }

    public static partial class EnumHelper
    {
        /// <summary>
        /// Lấy mô tả của enum.
        /// </summary>
        /// <param name="enumType">Loại enum.</param>
        /// <param name="value">Dữ liệu.</param>
        /// <returns>Chuỗi mô tả giá trị của enum.</returns>
        public static string? GetDescription(Type enumType, object value)
        {
            var valueName = value.ToString();
            if (valueName is null) return null;
            valueName = Enum.Parse(enumType, valueName).ToString();
            if (valueName is null) return null;
            var memberInfo = enumType.GetMember(valueName).FirstOrDefault();
            if (memberInfo is null) return null;

            var attrDesc = memberInfo.GetCustomAttribute<DescriptionAttribute>();
            return attrDesc is null ? valueName : attrDesc.Description;
        }

        /// <summary>
        /// Lấy mã code của enum.
        /// </summary>
        /// <param name="enumType">Loại enum.</param>
        /// <param name="value">Dữ liệu.</param>
        /// <returns>Chuỗi mã code của enum.</returns>
        public static string? GetCodeValue(Type enumType, object value)
        {
            var valueName = value.ToString();
            if (valueName is null) return null;
            valueName = Enum.Parse(enumType, valueName).ToString();
            if (valueName is null) return null;
            var memberInfo = enumType.GetMember(valueName).FirstOrDefault();
            if (memberInfo is null) return null;

            var attrDesc = memberInfo.GetCustomAttribute<CodeValueAttribute>();
            return attrDesc is null ? valueName : attrDesc.Value;
        }
    }
}