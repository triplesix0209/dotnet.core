using System.ComponentModel.DataAnnotations;
using System.Reflection;
using FluentValidation;
using FluentValidation.Validators;
using TripleSix.Core.Helpers;
using TripleSix.Core.Types;

namespace TripleSix.Core.Validation.Validators
{
    /// <summary>
    /// Required validator.
    /// </summary>
    /// <typeparam name="T">Data type.</typeparam>
    /// <typeparam name="TProperty">Property type.</typeparam>
    public class RequiredValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        /// <inheritdoc/>
        public override string Name => "RequiredValidator";

        /// <inheritdoc/>
        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            var instance = context.InstanceToValidate as object;
            var propertyName = context.PropertyPath;
            if (propertyName.Contains('.'))
            {
                var paths = propertyName.Split('.');
                propertyName = paths.Last();
                paths = paths[..^1];

                foreach (var path in paths)
                    instance = instance?.GetType().GetProperty(path)?.GetValue(instance);
            }

            if (instance is not IDto dto) return true;
            if (!dto.IsPropertyChanged(propertyName)) return false;

            var property = dto.GetType().GetProperty(propertyName);
            if (property == null) return true;

            if (property.PropertyType.IsNullableType()
                && property.GetCustomAttributes().Any(x => x.GetType().FullName == "System.Runtime.CompilerServices.NullableAttribute")
                && value == null)
                return false;

            var option = property.GetCustomAttribute<RequiredAttribute>();
            if (value is string str && option != null && !option.AllowEmptyStrings)
                return str.IsNotNullOrEmpty();

            return true;
        }

        /// <inheritdoc/>
        protected override string GetDefaultMessageTemplate(string errorCode)
            => "Bạn chưa nhập '{PropertyName}'";
    }
}
