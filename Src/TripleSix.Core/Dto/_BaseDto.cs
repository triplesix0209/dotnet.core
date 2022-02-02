#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TripleSix.Core.Exceptions;

namespace TripleSix.Core.Dto
{
    public abstract class BaseDto
        : IDto
    {
        public virtual object Clone()
        {
            var type = GetType();
            var result = Activator.CreateInstance(type);

            foreach (var p in type.GetProperties())
            {
                type.GetProperty(p.Name)?.SetValue(result, p.GetValue(this));
            }

            return result!;
        }

        public void Validate()
        {
            var errorDetail = new Dictionary<string, string[]>();

            var results = TryValidate();
            foreach (var result in results)
            {
                if (result.Value.Count <= 0) continue;
                errorDetail.Add(result.Key, result.Value.Select(x => x.ErrorMessage).ToArray());
            }

            if (errorDetail.Count == 0) return;
            throw new BaseException(BaseExceptions.BadClientRequest, errorDetail);
        }

        public virtual IDictionary<string, ICollection<ValidationResult>> TryValidate()
        {
            var result = new Dictionary<string, ICollection<ValidationResult>>();
            var context = new ValidationContext(this);

            var properties = GetType().GetProperties();
            foreach (var property in properties)
            {
                context.MemberName = property.Name;
                var validateResults = new List<ValidationResult>();
                Validator.TryValidateProperty(property.GetValue(this), context, validateResults);
                result.Add(context.MemberName, validateResults);
            }

            return result;
        }
    }
}
