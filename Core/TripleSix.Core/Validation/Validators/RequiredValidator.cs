using System.Collections;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Http;
using TripleSix.Core.Helpers;
using TripleSix.Core.Types;

namespace TripleSix.Core.Validation.Validators
{
    public class RequiredValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        public override string Name => "RequiredValidator";

        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            switch (value)
            {
                case null:
                case string str when str.IsNullOrWhiteSpace():
                case ICollection { Count: 0 }:
                case Array { Length: 0 }:
                case IEnumerable e when !e.GetEnumerator().MoveNext():
                    return false;
            }

            if (Equals(value, default(TProperty)))
                return false;

            if (context.InstanceToValidate is IDto dto)
            {
                var httpContext = context.RootContextData.ContainsKey(nameof(HttpContext))
                    ? context.RootContextData[nameof(HttpContext)] as HttpContext
                    : null;
                return httpContext?.Request.Method == HttpMethods.Put
                    || dto.IsPropertyChanged(context.PropertyName);
            }

            return true;
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
            => "Bạn chưa nhập '{PropertyName}'";
    }
}
