﻿using System.ComponentModel;
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
        private static readonly Dictionary<Type, IValidator?> _defaultValidators = new();
        private readonly HashSet<string> _propertyTracking = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        /// <param name="propertyName">Name of property.</param>
        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            SetPropertyChanged(propertyName, true);
        }

        public ValidationResult Validate(IValidator? validator = default, HttpContext? httpContext = default, bool throwOnFailures = false)
        {
            validator ??= GetDefaultValidator();
            if (validator == null) return new ValidationResult();

            var context = ValidationContext<IDto>.CreateWithOptions(this, options => { if (throwOnFailures) options.ThrowOnFailures(); });
            context.RootContextData[nameof(HttpContext)] = httpContext;

            return validator.Validate(context);
        }

        public virtual void Normalize()
        {
        }

        public virtual bool IsAnyPropertyChanged()
        {
            return _propertyTracking.Any();
        }

        public virtual bool IsPropertyChanged(string name)
        {
            return _propertyTracking.Any(x => x == name);
        }

        public virtual void SetPropertyChanged(string name, bool value)
        {
            if (value) _propertyTracking.Add(name);
            else _propertyTracking.RemoveWhere(x => x == name);
        }

        /// <summary>
        /// Lấy IValidator mặc định.
        /// </summary>
        /// <param name="dataType">Kiểu dữ liệu cần kiểm tra.</param>
        /// <returns><see cref="IValidator"/>.</returns>
        internal IValidator? GetDefaultValidator()
        {
            var dtoType = GetType();
            if (_defaultValidators.ContainsKey(dtoType))
                return _defaultValidators[dtoType];

            var validatorTypes = dtoType.Assembly
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

            _defaultValidators[dtoType] = Activator.CreateInstance(validatorType) as IValidator;
            return _defaultValidators[dtoType];
        }
    }
}
