using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CpTech.Core.Helpers;

namespace CpTech.Core.Attributes
{
    public class EnumValidateAttribute : ValidationAttribute
    {
        private const string _message = "giá trị của {0} không nằm trong tập giá trị cho phép";
        private readonly Type _enumType;

        public EnumValidateAttribute()
        {
            ErrorMessage = _message;
        }

        public EnumValidateAttribute(Type enumType)
        {
            ErrorMessage = _message;

            _enumType = Nullable.GetUnderlyingType(enumType) ?? enumType;
            if (!_enumType.IsEnum)
            {
                throw new ArgumentException("must be Enum type", nameof(enumType));
            }
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            Type type;
            if (_enumType == null)
            {
                type = value.GetType();
                type = Nullable.GetUnderlyingType(type) ?? type;
                if (!type.IsEnum) return false;
            }
            else
            {
                type = _enumType;
            }

            var values = EnumHelper.GetValues(type).Cast<int>();
            return values.Contains((int)value);
        }
    }
}