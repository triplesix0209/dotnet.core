using Autofac;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using TripleSix.Core.Validation;

namespace TripleSix.Core.Types
{
    /// <summary>
    /// DTO cơ bản.
    /// </summary>
    public abstract class BaseDto : IDto
    {
        private static IValidator? _defaultValidator;
        private readonly HashSet<string> _propertyTracking = new();

        /// <inheritdoc/>
        public ValidationResult Validate(IValidator? validator = default, HttpContext? httpContext = default, bool throwOnFailures = false)
        {
            if (validator == null) validator = GetDefaultValidator();
            if (validator == null) return new ValidationResult();

            var context = ValidationContext<IDto>.CreateWithOptions(this, options =>
            {
                if (throwOnFailures) options.ThrowOnFailures();
            });
            context.RootContextData[nameof(HttpContext)] = httpContext;

            return validator.Validate(context);
        }

        /// <inheritdoc/>
        public virtual bool IsAnyPropertyChanged()
        {
            return _propertyTracking.Any();
        }

        /// <inheritdoc/>
        public virtual bool IsPropertyChanged(string name)
        {
            return _propertyTracking.Any(x => x == name);
        }

        /// <inheritdoc/>
        public virtual void SetPropertyChanged(string name, bool value)
        {
            if (value) _propertyTracking.Add(name);
            else _propertyTracking.RemoveWhere(x => x == name);
        }

        /// <summary>
        /// Lấy IValidator mặc định.
        /// </summary>
        /// <returns><see cref="IValidator"/>.</returns>
        internal IValidator? GetDefaultValidator()
        {
            if (_defaultValidator != null)
                return _defaultValidator;

            var dtoType = GetType();
            var validatorTypes = GetType().Assembly
                .GetExportedTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => x.GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(IValidator<>) &&
                    i.GetGenericArguments()[0] == dtoType));
            if (validatorTypes.Count() > 1)
                throw new Exception($"More than one validator found for {dtoType.Name}");

            var validatorType = !validatorTypes.Any()
                ? typeof(BaseValidator<>).MakeGenericType(dtoType)
                : validatorTypes.First();

            _defaultValidator = Activator.CreateInstance(validatorType) as IValidator;
            return _defaultValidator;
        }
    }
}
