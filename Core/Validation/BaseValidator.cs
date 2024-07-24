using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Autofac;
using FluentValidation;
using TripleSix.Core.Helpers;
using TripleSix.Core.Types;
using TripleSix.Core.Validation.Validators;

namespace TripleSix.Core.Validation
{
    /// <summary>
    /// Validator cơ bản để kiểm tra dữ liệu.
    /// </summary>
    /// <typeparam name="T">Loại dữ liệu cần kiểm tra.</typeparam>
    public class BaseValidator<T> : AbstractValidator<T>
    {
        /// <summary>
        /// Khởi tạo <see cref="BaseValidator{T}"/>.
        /// </summary>
        public BaseValidator()
        {
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
                RuleForProperty(property, x => property.GetValue(x));
        }

        private void RuleForProperty(PropertyInfo property, Func<T, object?> valueGetter, string prefixName = "")
        {
            var propertyType = property.PropertyType.GetUnderlyingType();
            var propertyName = prefixName + property.Name;
            var propertyDisplayName = property.GetDisplayName();

            if (propertyType.IsEnum)
            {
                RuleFor(x => valueGetter(x))
                    .SetValidator(new EnumValidator<T, object?>(propertyType))
                    .OverridePropertyName(propertyName)
                    .WithName(propertyDisplayName);
            }

            if (property.GetCustomAttribute<RequiredAttribute>() != null)
            {
                RuleFor(x => valueGetter(x))
                    .SetValidator(new RequiredValidator<T, object?>())
                    .OverridePropertyName(propertyName)
                    .WithName(propertyDisplayName);
            }

            if (property.GetCustomAttribute<NotNullAttribute>() != null)
            {
                RuleFor(x => valueGetter(x))
                    .SetValidator(new NotNullValidator<T, object?>())
                    .OverridePropertyName(propertyName)
                    .WithName(propertyDisplayName);
            }

            if (property.GetCustomAttribute<NotEmptyAttribute>() != null)
            {
                RuleFor(x => valueGetter(x))
                    .SetValidator(new NotEmptyValidator<T, object?>())
                    .OverridePropertyName(propertyName)
                    .WithName(propertyDisplayName);
            }

            var minValueAttr = property.GetCustomAttribute<MinValueAttribute>();
            if (minValueAttr != null)
            {
                RuleFor(x => valueGetter(x))
                    .SetValidator(new MinValueValidator<T, object?>(minValueAttr.Value))
                    .OverridePropertyName(propertyName)
                    .WithName(propertyDisplayName);
            }

            var maxValueAttr = property.GetCustomAttribute<MaxValueAttribute>();
            if (maxValueAttr != null)
            {
                RuleFor(x => valueGetter(x))
                    .SetValidator(new MaxValueValidator<T, object?>(maxValueAttr.Value))
                    .OverridePropertyName(propertyName)
                    .WithName(propertyDisplayName);
            }

            var minLengthAttr = property.GetCustomAttribute<MinLengthAttribute>();
            if (minLengthAttr != null)
            {
                RuleFor(x => valueGetter(x))
                    .SetValidator(new MinLengthValidator<T, object?>(minLengthAttr.Length))
                    .OverridePropertyName(propertyName)
                    .WithName(propertyDisplayName);
            }

            var maxLengthAttr = property.GetCustomAttribute<MaxLengthAttribute>();
            if (maxLengthAttr != null)
            {
                RuleFor(x => valueGetter(x))
                    .SetValidator(new MaxLengthValidator<T, object?>(maxLengthAttr.Length))
                    .OverridePropertyName(propertyName)
                    .WithName(propertyDisplayName);
            }

            var mustNoSpaceAttr = property.GetCustomAttribute<MustNoSpaceAttribute>();
            if (mustNoSpaceAttr != null)
            {
                RuleFor(x => valueGetter(x))
                    .SetValidator(new MustNoSpaceValidator<T, object?>())
                    .OverridePropertyName(propertyName)
                    .WithName(propertyDisplayName);
            }

            var mustTrimAttr = property.GetCustomAttribute<MustTrimAttribute>();
            if (mustTrimAttr != null)
            {
                RuleFor(x => valueGetter(x))
                    .SetValidator(new MustTrimValidator<T, object?>())
                    .OverridePropertyName(propertyName)
                    .WithName(propertyDisplayName);
            }

            var mustLowerCaseAttr = property.GetCustomAttribute<MustLowerCaseAttribute>();
            if (mustLowerCaseAttr != null)
            {
                RuleFor(x => valueGetter(x))
                    .SetValidator(new MustLowerCaseValidator<T, object?>())
                    .OverridePropertyName(propertyName)
                    .WithName(propertyDisplayName);
            }

            var mustUpperCaseAttr = property.GetCustomAttribute<MustUpperCaseAttribute>();
            if (mustUpperCaseAttr != null)
            {
                RuleFor(x => valueGetter(x))
                    .SetValidator(new MustUpperCaseValidator<T, object?>())
                    .OverridePropertyName(propertyName)
                    .WithName(propertyDisplayName);
            }

            var mustWordNumberAttr = property.GetCustomAttribute<MustWordNumberAttribute>();
            if (mustWordNumberAttr != null)
            {
                RuleFor(x => valueGetter(x))
                    .SetValidator(new MustWordNumberValidator<T, object?>())
                    .OverridePropertyName(propertyName)
                    .WithName(propertyDisplayName);
            }

            var mustNumberAttr = property.GetCustomAttribute<MustNumberAttribute>();
            if (mustNumberAttr != null)
            {
                RuleFor(x => valueGetter(x))
                    .SetValidator(new MustNumberValidator<T, object?>())
                    .OverridePropertyName(propertyName)
                    .WithName(propertyDisplayName);
            }

            var mustEmailAttr = property.GetCustomAttribute<MustEmailAttribute>();
            if (mustEmailAttr != null)
            {
                RuleFor(x => valueGetter(x))
                    .SetValidator(new MustEmailValidator<T, object?>())
                    .OverridePropertyName(propertyName)
                    .WithName(propertyDisplayName);
            }

            var mustPhoneAttr = property.GetCustomAttribute<MustPhoneAttribute>();
            if (mustPhoneAttr != null)
            {
                RuleFor(x => valueGetter(x))
                    .SetValidator(new MustPhoneValidator<T, object?>())
                    .OverridePropertyName(propertyName)
                    .WithName(propertyDisplayName);
            }

            var mustRegExrAttr = property.GetCustomAttribute<MustRegExrAttribute>();
            if (mustRegExrAttr != null)
            {
                RuleFor(x => valueGetter(x))
                    .SetValidator(new MustRegExrValidator<T, object?>(mustRegExrAttr.PatternExr, mustRegExrAttr.PatternName))
                    .OverridePropertyName(propertyName)
                    .WithName(propertyDisplayName);
            }

            if (propertyType.IsAssignableTo<IDto>())
            {
                foreach (var childProperty in propertyType.GetProperties())
                {
                    RuleForProperty(
                        childProperty,
                        x => valueGetter(x) == null ? null : childProperty.GetValue(valueGetter(x)),
                        $"{property.Name}.");
                }
            }
        }
    }

    /// <summary>
    /// Base validator.
    /// </summary>
    public class BaseValidator
    {
        /// <summary>
        /// Cài đặt các cấu hình validator.
        /// </summary>
        public static void SetupGlobal()
        {
            ValidatorOptions.Global.DisplayNameResolver = (type, member, expression) =>
            {
                if (member == null) return null;
                return member.GetDisplayName();
            };
        }

        /// <summary>
        /// Kiểm tra các validator của dto có lỗi hay không.
        /// </summary>
        /// <param name="assembly">Assembly chứa các dto validator cần kiểm tra.</param>
        public static void ValidateDtoValidator(Assembly assembly)
        {
            var dtoTypes = assembly.GetExportedTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsAssignableTo<BaseDto>());

            foreach (var dtoType in dtoTypes)
            {
                var dto = Activator.CreateInstance(dtoType) as BaseDto;
                if (dto == null) continue;
                dto.GetDefaultValidator();
            }
        }
    }
}
