using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Autofac;
using FluentValidation;
using TripleSix.Core.Helpers;
using TripleSix.Core.Types;
using TripleSix.Core.Validators.PropertyValidators;

namespace TripleSix.Core.Validators
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
                RuleForProperty(property);
        }

        private void RuleForProperty(PropertyInfo property)
        {
            if (property.GetCustomAttribute<RequiredAttribute>() != null)
            {
                RuleFor(x => property.GetValue(x))
                    .SetValidator(new RequiredValidator<T, object?>())
                    .OverridePropertyName(property.Name)
                    .WithName(property.GetDisplayName());
            }
        }
    }

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
