using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CpTech.Core.Helpers;

namespace CpTech.Core.Attributes
{
    public class EnumValidateAttribute : ValidationAttribute
    {
        private readonly Type _enumType;

        public EnumValidateAttribute()
        {
            ErrorMessage = "giá trị của {0} không nằm trong tập giá trị cho phép";
        }

        public EnumValidateAttribute(Type enumType)
        {
            _enumType = Nullable.GetUnderlyingType(enumType) ?? enumType;
            if (!_enumType.IsEnum)
            {
                throw new ArgumentException("must be Enum type", nameof(enumType));
            }

            ErrorMessage = "giá trị của {0} không nằm trong tập giá trị cho phép";
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
                if (!type.IsEnum)
                {
                    return true;
                }
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